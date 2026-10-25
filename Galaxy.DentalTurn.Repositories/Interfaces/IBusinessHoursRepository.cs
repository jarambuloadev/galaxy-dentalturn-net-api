using Galaxy.DentalTurn.DataAccess.Entities;

namespace Galaxy.DentalTurn.Repositories.Interfaces
{
    public interface IBusinessHoursRepository : IBaseRepository<BusinessHours>
    {
        Task<List<BusinessHours>> GetAllOrderedByDayAsync();
    }
}
