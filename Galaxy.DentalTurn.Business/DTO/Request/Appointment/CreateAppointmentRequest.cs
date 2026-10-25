namespace Galaxy.DentalTurn.Business.DTO.Request.Appointments
{
    public class CreateAppointmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public int PractitionerId { get; set; }
        public string Date { get; set; } = string.Empty; // "YYYY-MM-DD"
        public string StartTime { get; set; } = string.Empty; // "HH:mm" o ISO
        public string? Notes { get; set; }
    }

    public class UpdateAppointmentStatusRequest
    {
        public string Status { get; set; } = string.Empty; // PENDING, CONFIRMED, CANCELLED, COMPLETED
    }

    public class AppointmentFilterRequest
    {
        public string? Date { get; set; }
        public int? ServiceId { get; set; }
        public int? PractitionerId { get; set; }
        public string? Status { get; set; }
    }
}
