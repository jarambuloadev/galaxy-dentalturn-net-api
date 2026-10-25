using Galaxy.DentalTurn.Business.DTO.Request;
using Galaxy.DentalTurn.Business.DTO.Request.AdminUser;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Business.Interfaces;
using Galaxy.DentalTurn.Common.Helpers;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Business.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminUserService(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        public async Task<Result> RegisterAsync(CreateAdminUserRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var userNameExists = await _adminUserRepository.ExistsByNameAsync(request.Name);
                if (userNameExists)
                {
                    return Result.Failure($"El nombre de usuario {request.Name} ya existe.");
                }
            }

            var userEmailExists = await _adminUserRepository.GetByPredicateAsync(u => u.Status && u.Email == request.Email);
            if (userEmailExists != null)
            {
                return Result.Failure($"El email {request.Email} ya existe.");
            }

            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            var user = new AdminUser
            {
                Email = request.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                Name = request.Name?.Trim() ?? request.Email.Trim(),
                Rol = string.IsNullOrWhiteSpace(request.Rol) ? "admin" : request.Rol.Trim().ToLowerInvariant(),
                Status = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _adminUserRepository.CreateAsync(user);
            return Result.Success("Usuario registrado exitosamente.");
        }

        public async Task<Result<GetAdminUserResponse>> GetByIdAsync(int id)
        {
            var user = await _adminUserRepository.GetByIdAsync(id);
            if (user is null)
            {
                return Result.Failure<GetAdminUserResponse>($"Usuario con id {id} no encontrado.");
            }

            var response = new GetAdminUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name ?? string.Empty,
                Rol = user.Rol,
                Active = user.Status,
                CreatedAt = user.CreatedAt,
                CreatedBy = user.CreatedBy
            };

            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(int id, UpdateAdminUserRequest request)
        {
            var existing = await _adminUserRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return Result.Failure($"Usuario con id {id} no encontrado.");
            }

            var duplicateEmail = await _adminUserRepository.GetByPredicateAsync(
                u => u.Status && u.Id != id && u.Email == request.Email.Trim());
            if (duplicateEmail != null)
            {
                return Result.Failure($"El email {request.Email} ya está en uso por otro usuario.");
            }

            existing.Email = request.Email.Trim();
            existing.Name = string.IsNullOrWhiteSpace(request.Name) ? existing.Email : request.Name.Trim();
            if (!string.IsNullOrWhiteSpace(request.Rol))
            {
                existing.Rol = request.Rol.Trim().ToLowerInvariant();
            }

            if (!string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            }

            existing.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            await _adminUserRepository.UpdateAsync();
            return Result.Success("Usuario actualizado exitosamente.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var existing = await _adminUserRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return Result.Failure($"Usuario con id {id} no encontrado.");
            }

            await _adminUserRepository.DeleteAsync(id);
            return Result.Success("Usuario eliminado exitosamente.");
        }

        public async Task<Result<List<ListAdminUsersResponse>>> ListAsync(PagedRequest request)
        {
            var result = await _adminUserRepository.ListAsync(
                predicate: u => u.Status &&
                    (string.IsNullOrEmpty(request.Search) ||
                     (u.Name != null && u.Name.Contains(request.Search)) ||
                     u.Email.Contains(request.Search) ||
                     u.Rol.Contains(request.Search)),
                selector: u => new ListAdminUsersResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name ?? string.Empty,
                    Rol = u.Rol,
                    Active = u.Status
                },
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

            return Result.Success(result.Result.ToList());
        }
    }
}
