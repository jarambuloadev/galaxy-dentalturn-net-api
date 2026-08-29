using System;

namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class GetPatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = Environment.UserName;
    }
}
