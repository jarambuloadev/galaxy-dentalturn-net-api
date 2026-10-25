using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class BusinessHoursRepository(DentalTurnDbContext context)
        : BaseRepository<BusinessHours>(context), IBusinessHoursRepository
    {
        public async Task<List<BusinessHours>> GetAllOrderedByDayAsync()
        {
            return await _context.BusinessHours
                .OrderBy(b => b.DayOfWeek)
                .ToListAsync();
        }
    }
}
