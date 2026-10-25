namespace Galaxy.DentalTurn.Business.DTO.Request.AdminUser
{
    public class UpdateAdminUserRequest
    {
        public string Email { get; set; } = default!;
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? Rol { get; set; }
    }
}
