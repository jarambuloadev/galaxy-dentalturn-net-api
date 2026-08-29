using Galaxy.DentalTurn.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.DTO.Request.Service
{
    public class UpdateServiceRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public int DurationMin { get; set; } = 30;
        public bool Active { get; set; } = true;

        public ICollection<PractitionerService> Practitioners { get; set; } = new List<PractitionerService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
