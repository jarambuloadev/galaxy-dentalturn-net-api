namespace Galaxy.DentalTurn.Business.DTO.Request.Auth
{
    public class LoginRequest
    {
        public string email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
