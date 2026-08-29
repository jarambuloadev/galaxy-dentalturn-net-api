using Galaxy.DentalTurn.DataAccess.Entities;

namespace Galaxy.DentalTurn.Repositories.Interfaces
{
    public interface IAdminUserRepository : IBaseRepository<AdminUser>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
