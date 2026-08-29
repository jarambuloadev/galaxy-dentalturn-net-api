using Galaxy.DentalTurn.Business.DTO.Response.Auth;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface ITokenService
    {
        Result<TokenResponse> Generate(AdminUser request);
    }
}
