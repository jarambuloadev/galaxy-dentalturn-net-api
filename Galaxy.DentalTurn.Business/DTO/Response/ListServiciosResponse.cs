using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class ListServiciosResponse
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMin { get; set; }
        public bool Active { get; set; }
    }
}
