using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.Patient;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Mapster;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Result> CreateAsync(UpdatePatientRequest request)
        {
            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            var patient = new Patient
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone.Trim(),
                Status = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _patientRepository.CreateAsync(patient);
            return Result.Success("Paciente creado correctamente.");
        }

        public async Task<Result<GetPatientResponse>> GetByIdAsync(int id)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Paciente con id {id} no encontrado.");
            }
            var response = new GetPatientResponse
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                Phone = result.Phone,
                Active = result.Status,
                CreatedAt = result.CreatedAt,
                CreatedBy = result.CreatedBy
            };
            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(int id, UpdatePatientRequest request)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Paciente con id {id} no encontrado.");
            }
            result.Name = request.Name.Trim();
            result.Email = request.Email.Trim();
            result.Phone = request.Phone.Trim();
            result.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            await _patientRepository.UpdateAsync();
            return Result.Success("Paciente actualizado correctamente.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Paciente con id {id} no encontrado.");
            }
            await _patientRepository.DeleteAsync(id);
            return Result.Success("Paciente eliminado correctamente.");
        }

        public async Task<Result<List<ListPatientsResponse>>> ListAsync(PagedRequest request)
        {
            var result = await _patientRepository.ListAsync(
                predicate: p => p.Status &&
                    (string.IsNullOrEmpty(request.Search) ||
                     p.Name.Contains(request.Search) ||
                     p.Email.Contains(request.Search) ||
                     p.Phone.Contains(request.Search)),
                selector: p => new ListPatientsResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Phone = p.Phone,
                    Active = p.Status
                },
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

            return Result.Success(result.Result.ToList());
        }
    }
}
