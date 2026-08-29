using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.DentalTurn.Business.DTO.Request.Patient
{
    public class UpdatePatientRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
