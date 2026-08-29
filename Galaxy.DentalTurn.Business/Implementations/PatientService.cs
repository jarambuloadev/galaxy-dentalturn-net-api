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
            var patient = request.Adapt<Patient>();
            var result = await _patientRepository.CreateAsync(patient);
            return Result.Success("Patient created successfully.");
        }

        public async Task<Result<GetPatientResponse>> GetByIdAsync(int id)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Patient with id {id} not found.");
            }
            return Result.Success(result.Adapt<GetPatientResponse>());
        }

        public async Task<Result> UpdateAsync(int id, UpdatePatientRequest request)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Patient with id {id} not found.");
            }
            var patient = request.Adapt(result);
            await _patientRepository.UpdateAsync();
            return Result.Success("Patient updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var result = await _patientRepository.GetByIdAsync(id);
            if (result is null)
            {
                return Result.Failure<GetPatientResponse>($"Patient with id {id} not found.");
            }
            await _patientRepository.DeleteAsync(id);
            return Result.Success("Patient deleted successfully.");
        }

        public async Task<Result<List<ListPatientsResponse>>> ListAsync(int pageNumnber = 1, int pageSize = 10)
        {
            var result = await _patientRepository.ListAsync(
                predicate: p => p.Status,
                selector: p => new ListPatientsResponse
                {
                    Name = p.Name,
                    Email = p.Email,
                    Phone = p.Phone,
                    Active = p.Status
                },
                pageNumber: pageNumnber,
                pageSize: pageSize);

            return Result.Success(result.Result.ToList());
        }
    }
}
