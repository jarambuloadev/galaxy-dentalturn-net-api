using Galaxy.DentalTurn.Business.DTO.Request.Appointments;
using Galaxy.DentalTurn.Business.DTO.Response;
using Galaxy.DentalTurn.Common.Helpers;

namespace Galaxy.DentalTurn.Business.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<List<AvailableSlotResponse>>> GetAvailableSlotsAsync(string date, int serviceId, int practitionerId);
        Task<Result<AppointmentResponse>> CreateAsync(CreateAppointmentRequest request);
        Task<Result<AppointmentResponse>> GetByIdAsync(int id);
        Task<Result<List<AppointmentResponse>>> ListAsync(AppointmentFilterRequest filter);
        Task<Result> UpdateStatusAsync(int id, string status);
    }
}
