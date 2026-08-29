using Galaxy.DentalTurn.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class GetServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int DurationMin { get; set; } = 30;
        public bool Active { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; } = Environment.UserName;        

    }
}
