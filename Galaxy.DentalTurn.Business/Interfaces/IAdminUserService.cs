
using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IAdminUserService
    {
        Task<Result> RegisterAsync(CreateAdminUserRequest request);
    }
}
