using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.Practitioner;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IPractitionerService
    {
        Task<Result> CreateAsync(UpdatePractitionerRequest request);
        Task<Result<GetPractitionerResponse>> GetByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdatePractitionerRequest request);
        Task<Result> DeleteAsync(int id);
        Task<Result<List<ListPractitionersResponse>>> ListAsync(PagedRequest request);
    }
}
