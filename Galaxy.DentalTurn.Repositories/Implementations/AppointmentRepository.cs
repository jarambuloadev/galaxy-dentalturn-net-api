using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class AppointmentRepository(DentalTurnDbContext context) : BaseRepository<Appointment>(context), IAppointmentRepository
    {
        public async Task<Appointment?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Practitioner)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> ListWithDetailsAsync(
            DateOnly? date,
            int? serviceId,
            int? practitionerId,
            AppointmentStatus? status)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Practitioner)
                .AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(a => a.Date == date.Value);
            }

            if (serviceId.HasValue)
            {
                query = query.Where(a => a.ServiceId == serviceId.Value);
            }

            if (practitionerId.HasValue)
            {
                query = query.Where(a => a.PractitionerId == practitionerId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.AppointmentStatus == status.Value);
            }

            return await query
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetActiveByPractitionerAndDateAsync(int practitionerId, DateOnly date)
        {
            return await _context.Appointments
                .Where(a =>
                    a.PractitionerId == practitionerId &&
                    a.Date == date &&
                    (a.AppointmentStatus == AppointmentStatus.PENDING || a.AppointmentStatus == AppointmentStatus.CONFIRMED))
                .ToListAsync();
        }
    }
}
