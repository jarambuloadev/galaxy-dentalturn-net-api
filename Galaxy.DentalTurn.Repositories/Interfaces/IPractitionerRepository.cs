using Galaxy.DentalTurn.DataAccess.Entities;

namespace Galaxy.DentalTurn.Repositories.Interfaces
{
    public interface IPractitionerRepository : IBaseRepository<Practitioner>
    {
        Task<Practitioner?> GetByIdWithServicesAsync(int id);
        Task UpdateServicesAsync(int practitionerId, List<int> serviceIds);
    }
}
