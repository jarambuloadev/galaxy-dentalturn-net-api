namespace Galaxy.DentalTurn.Business.DTO.Request.AdminUser
{
    public class CreateAdminUserRequest
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? Name { get; set; } = default!;
    }
}
