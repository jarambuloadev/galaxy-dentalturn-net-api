using System.Globalization;
using Galaxy.DentalTurn.Business.DTO.Request.Appointments;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private const int SlotDurationMin = 30;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPractitionerRepository _practitionerRepository;
        private readonly IBusinessHoursRepository _businessHoursRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IServiceRepository serviceRepository,
            IPractitionerRepository practitionerRepository,
            IBusinessHoursRepository businessHoursRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _serviceRepository = serviceRepository;
            _practitionerRepository = practitionerRepository;
            _businessHoursRepository = businessHoursRepository;
        }

        public async Task<Result<List<AvailableSlotResponse>>> GetAvailableSlotsAsync(string date, int serviceId, int practitionerId)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return Result.Failure<List<AvailableSlotResponse>>("Fecha inválida.");
            }

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service is null || !service.Status)
            {
                return Result.Success(new List<AvailableSlotResponse>());
            }

            var dayOfWeek = (int)parsedDate.DayOfWeek;
            var allHours = await _businessHoursRepository.GetAllOrderedByDayAsync();
            var dayHours = allHours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);

            if (dayHours is null || !dayHours.IsOpen)
            {
                return Result.Success(new List<AvailableSlotResponse>());
            }

            if (!TimeOnly.TryParseExact(dayHours.OpenTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var openTime) ||
                !TimeOnly.TryParseExact(dayHours.CloseTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var closeTime))
            {
                return Result.Success(new List<AvailableSlotResponse>());
            }

            var dayClose = DateTime.SpecifyKind(parsedDate.ToDateTime(closeTime), DateTimeKind.Unspecified);
            var cursor = DateTime.SpecifyKind(parsedDate.ToDateTime(openTime), DateTimeKind.Unspecified);
            var now = DateTime.Now;

            var existingAppointments = await _appointmentRepository.GetActiveByPractitionerAndDateAsync(practitionerId, parsedDate);
            var slots = new List<AvailableSlotResponse>();

            while (cursor < dayClose)
            {
                var slotStart = cursor;
                var slotEnd = slotStart.AddMinutes(service.DurationMin);

                var fitsBeforeClose = slotEnd <= dayClose;
                var isInFuture = slotStart > now;
                var overlapsExisting = existingAppointments.Any(a => slotStart < a.EndTime && slotEnd > a.StartTime);

                if (fitsBeforeClose && isInFuture && !overlapsExisting)
                {
                    slots.Add(new AvailableSlotResponse
                    {
                        StartTime = slotStart.ToString("yyyy-MM-ddTHH:mm:ss"),
                        EndTime = slotEnd.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Label = slotStart.ToString("HH:mm")
                    });
                }

                cursor = cursor.AddMinutes(SlotDurationMin);
            }

            return Result.Success(slots);
        }

        public async Task<Result<AppointmentResponse>> CreateAsync(CreateAppointmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Phone))
            {
                return Result.Failure<AppointmentResponse>("Completa todos tus datos de contacto.");
            }

            if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return Result.Failure<AppointmentResponse>("Fecha inválida.");
            }

            var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
            if (service is null || !service.Status)
            {
                return Result.Failure<AppointmentResponse>("El servicio elegido no está disponible.");
            }

            var practitioner = await _practitionerRepository.GetByIdAsync(request.PractitionerId);
            if (practitioner is null || !practitioner.Status || !practitioner.Active)
            {
                return Result.Failure<AppointmentResponse>("El odontólogo elegido no está disponible.");
            }

            DateTime startTime;
            if (TimeOnly.TryParseExact(request.StartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeOnly))
            {
                startTime = DateTime.SpecifyKind(parsedDate.ToDateTime(timeOnly), DateTimeKind.Unspecified);
            }
            else if (DateTime.TryParse(request.StartTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
            {
                startTime = DateTime.SpecifyKind(parsedDateTime, DateTimeKind.Unspecified);
            }
            else
            {
                return Result.Failure<AppointmentResponse>("Horario inválido. Elige otro horario.");
            }

            var endTime = startTime.AddMinutes(service.DurationMin);

            var existingAppointments = await _appointmentRepository.GetActiveByPractitionerAndDateAsync(request.PractitionerId, parsedDate);
            var overlaps = existingAppointments.Any(a => startTime < a.EndTime && endTime > a.StartTime);
            if (overlaps)
            {
                return Result.Failure<AppointmentResponse>("Ese horario ya no está disponible. Elige otro horario.");
            }

            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            var patient = await _patientRepository.CreateAsync(new Patient
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone.Trim(),
                Status = true,
                CreatedAt = now,
                UpdatedAt = now
            });

            var appointment = await _appointmentRepository.CreateAsync(new Appointment
            {
                PatientId = patient.Id,
                ServiceId = service.Id,
                PractitionerId = practitioner.Id,
                Date = parsedDate,
                StartTime = startTime,
                EndTime = endTime,
                AppointmentStatus = AppointmentStatus.PENDING,
                Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
                CreatedAt = now,
                UpdatedAt = now
            });

            var created = await _appointmentRepository.GetByIdWithDetailsAsync(appointment.Id);
            return Result.Success(MapToResponse(created ?? appointment, patient, service, practitioner));
        }

        public async Task<Result<AppointmentResponse>> GetByIdAsync(int id)
        {
            var appt = await _appointmentRepository.GetByIdWithDetailsAsync(id);
            if (appt is null)
            {
                return Result.Failure<AppointmentResponse>($"Turno con id {id} no encontrado.");
            }

            return Result.Success(MapToResponse(appt, appt.Patient, appt.Service, appt.Practitioner));
        }

        public async Task<Result<List<AppointmentResponse>>> ListAsync(AppointmentFilterRequest filter)
        {
            DateOnly? parsedDate = null;
            if (!string.IsNullOrWhiteSpace(filter.Date) &&
                DateOnly.TryParseExact(filter.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            {
                parsedDate = d;
            }

            AppointmentStatus? parsedStatus = null;
            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<AppointmentStatus>(filter.Status, true, out var st))
            {
                parsedStatus = st;
            }

            var list = await _appointmentRepository.ListWithDetailsAsync(
                parsedDate,
                filter.ServiceId,
                filter.PractitionerId,
                parsedStatus);

            var response = list
                .Select(a => MapToResponse(a, a.Patient, a.Service, a.Practitioner))
                .ToList();

            return Result.Success(response);
        }

        public async Task<Result> UpdateStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<AppointmentStatus>(status, true, out var parsedStatus))
            {
                return Result.Failure("Estado de turno inválido.");
            }

            var appt = await _appointmentRepository.GetByIdWithDetailsAsync(id);
            if (appt is null)
            {
                return Result.Failure($"Turno con id {id} no encontrado.");
            }

            appt.AppointmentStatus = parsedStatus;
            appt.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            await _appointmentRepository.UpdateAsync();

            return Result.Success("Estado del turno actualizado correctamente.");
        }

        private static AppointmentResponse MapToResponse(
            Appointment a,
            Patient? patient,
            Service? service,
            Practitioner? practitioner)
        {
            return new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = patient?.Name ?? string.Empty,
                PatientEmail = patient?.Email ?? string.Empty,
                PatientPhone = patient?.Phone ?? string.Empty,
                ServiceId = a.ServiceId,
                ServiceName = service?.Name ?? string.Empty,
                PractitionerId = a.PractitionerId,
                PractitionerName = practitioner?.Name ?? string.Empty,
                Date = a.Date.ToString("yyyy-MM-dd"),
                DateLabel = a.StartTime.ToString("dd/MM/yyyy"),
                TimeLabel = a.StartTime.ToString("HH:mm"),
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.AppointmentStatus.ToString(),
                Notes = a.Notes
            };
        }
    }
}
