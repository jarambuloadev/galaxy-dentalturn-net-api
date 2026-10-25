using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IAdminUserService
    {
        Task<Result> RegisterAsync(CreateAdminUserRequest request);
        Task<Result<GetAdminUserResponse>> GetByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdateAdminUserRequest request);
        Task<Result> DeleteAsync(int id);
        Task<Result<List<ListAdminUsersResponse>>> ListAsync(PagedRequest request);
    }
}
