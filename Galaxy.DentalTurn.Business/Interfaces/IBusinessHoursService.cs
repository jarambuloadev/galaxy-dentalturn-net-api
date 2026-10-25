using Galaxy.DentalTurn.Business.DTO.Request.BusinessHours;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IBusinessHoursService
    {
        Task<Result<List<BusinessHoursResponse>>> ListAsync();
        Task<Result> UpdateAllAsync(UpdateBusinessHoursRequest request);
    }
}
