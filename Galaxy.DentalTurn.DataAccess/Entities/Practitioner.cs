namespace Galaxy.DentalTurn.DataAccess.Entities;

public class Practitioner : BaseEntity
{
    public string Name { get; set; } = null!;
    public bool Active { get; set; } = true;    

    public ICollection<PractitionerService> Services { get; set; } = new List<PractitionerService>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
