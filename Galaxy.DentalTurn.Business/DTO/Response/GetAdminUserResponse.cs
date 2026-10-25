using System;

namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class GetAdminUserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Rol { get; set; } = "admin";
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = Environment.UserName;
    }
}
