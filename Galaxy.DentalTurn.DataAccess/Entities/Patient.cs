namespace Galaxy.DentalTurn.DataAccess.Entities;

public class Patient : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
   
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
