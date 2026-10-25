namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class AvailableSlotResponse
    {
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class AppointmentResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int PractitionerId { get; set; }
        public string PractitionerName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string DateLabel { get; set; } = string.Empty;
        public string TimeLabel { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? Notes { get; set; }
    }
}
