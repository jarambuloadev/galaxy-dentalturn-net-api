using Galaxy.DentalTurn.Business.DTO.Request.Auth;
using Galaxy.DentalTurn.Business.DTO.Response.Auth;
using Galaxy.DentalTurn.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
