using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.Business.DTO.Request.Auth;
using Galaxy.DentalTurn.Business.DTO.Response.Auth;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class AuthService: IAuthService
    {
        private readonly IAdminUserRepository _adminuserRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IAdminUserRepository adminUserRepository, ITokenService tokenService)
        {
            _adminuserRepository = adminUserRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            //Validación de credenciales
            var user = await _adminuserRepository.GetByPredicateAsync(p => p.Email == request.email);

            if (user is null)
                return Result.Failure<LoginResponse>("Usuario no existe");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Result.Failure<LoginResponse>("Contraseña incorrecta");

            //Generación de token
            var result = _tokenService.Generate(user);
            if (result.IsFailure)
                return Result.Failure<LoginResponse>(result.Message ?? "Error al generar token");

            var tokenDto = result.Value;
            var response = new LoginResponse
            {
                Token = tokenDto.Token,
                ExpirationDate = tokenDto.ExpirationDate,
                // Role = user.Rol
            };

            return Result.Success(response);
        }
    }
}
