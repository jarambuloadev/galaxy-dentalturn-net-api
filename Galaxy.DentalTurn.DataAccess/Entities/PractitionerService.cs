namespace Galaxy.DentalTurn.DataAccess.Entities;

public class PractitionerService : BaseEntity
{
    public int PractitionerId { get; set; }
    public Practitioner Practitioner { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}
