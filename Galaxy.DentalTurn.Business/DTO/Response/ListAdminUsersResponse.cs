namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class ListAdminUsersResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Rol { get; set; } = "admin";
        public bool Active { get; set; } = true;
    }
}
