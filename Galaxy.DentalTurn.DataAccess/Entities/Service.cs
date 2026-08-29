namespace Galaxy.DentalTurn.DataAccess.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationMin { get; set; } = 30;
    public bool Active { get; set; } = true;

    public ICollection<PractitionerService> Practitioners { get; set; } = new List<PractitionerService>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
