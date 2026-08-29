using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class AdminUserRepository(DentalTurnDbContext context) : BaseRepository<AdminUser>(context), IAdminUserRepository
    {
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Set<AdminUser>().AnyAsync(u => u.Name == name);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Set<AdminUser>().AnyAsync(u => u.Email == email);
        }   
    }
}
