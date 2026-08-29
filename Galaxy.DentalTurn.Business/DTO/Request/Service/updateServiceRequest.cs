using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.DTO.Request.Service
{
    public class updateServiceRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public int DurationMin { get; set; } = 30;        
    }
}
