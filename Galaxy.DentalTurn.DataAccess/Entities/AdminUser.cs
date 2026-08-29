namespace Galaxy.DentalTurn.DataAccess.Entities;

public class AdminUser : BaseEntity
{    
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? Name { get; set; } = default!;
}
