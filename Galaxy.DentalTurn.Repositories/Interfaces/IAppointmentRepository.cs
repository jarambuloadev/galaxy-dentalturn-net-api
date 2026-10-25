using Galaxy.DentalTurn.DataAccess.Entities;

namespace Galaxy.DentalTurn.Repositories.Interfaces
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<Appointment?> GetByIdWithDetailsAsync(int id);
        Task<List<Appointment>> ListWithDetailsAsync(
            DateOnly? date,
            int? serviceId,
            int? practitionerId,
            AppointmentStatus? status);
        Task<List<Appointment>> GetActiveByPractitionerAndDateAsync(int practitionerId, DateOnly date);
    }
}
