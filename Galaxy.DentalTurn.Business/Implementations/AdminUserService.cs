using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Mapster;


namespace Galaxy.DentalTurn.Business.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminUserService(IAdminUserRepository adminUserRepository)
        {
            this._adminUserRepository = adminUserRepository;
        }

        public async Task<Result> RegisterAsync(CreateAdminUserRequest request)
        {
            // Validacion de existencia de usuario
            var userNameExists = await _adminUserRepository.ExistsByNameAsync(request.Name);
            if(userNameExists)
            {
                return Result.Failure($"El nombre de usuario {request.Name} ya existe.");
            }

            //Validacion de existencia de email

            var userEmailExists = await _adminUserRepository.GetByPredicateAsync(u => u.Email == request.Email);
            if (userEmailExists != null)
            {
                return Result.Failure($"El email {request.Email} ya existe.");
            }

            var user = new AdminUser
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword( request.PasswordHash),
                Name = request.Name,
                Rol = string.IsNullOrWhiteSpace(request.Rol) ? "admin" : request.Rol
            };

            await _adminUserRepository.CreateAsync(user);
            return Result.Success("Usuario registrado exitosamente.");
        }
    }
}
