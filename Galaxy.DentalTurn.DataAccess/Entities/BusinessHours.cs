namespace Galaxy.DentalTurn.DataAccess.Entities;

public class BusinessHours : BaseEntity
{
    
    public int DayOfWeek { get; set; }
    public bool IsOpen { get; set; } = true;
    public string OpenTime { get; set; } = null!;
    public string CloseTime { get; set; } = null!;    
}
