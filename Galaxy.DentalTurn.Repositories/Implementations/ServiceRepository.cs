
using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class ServiceRepository(DentalTurnDbContext context) : BaseRepository<Service>(context), IServiceRepository
    {
    }
}
