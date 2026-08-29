namespace Galaxy.DentalTurn.DataAccess.Entities;

public class Appointment : BaseEntity
{
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public int PractitionerId { get; set; }
    public Practitioner? Practitioner { get; set; }

    public DateOnly Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.PENDING;
    public string? Notes { get; set; }

}
