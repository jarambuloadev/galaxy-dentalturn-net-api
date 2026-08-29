using Galaxy.DentalTurn.Business.DTO.Request.Patient;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;
using System.Collections.Generic;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IPatientService
    {
        Task<Result> CreateAsync(UpdatePatientRequest request);
        Task<Result<GetPatientResponse>> GetByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdatePatientRequest request);
        Task<Result> DeleteAsync(int id);
        Task<Result<List<ListPatientsResponse>>> ListAsync(int pageNumnber = 1, int pageSize = 10);
    }
}
