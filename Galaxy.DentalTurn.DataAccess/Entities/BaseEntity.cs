using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Galaxy.DentalTurn.DataAccess.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; } = Environment.UserName;        
        public DateTime UpdatedAt { get; set; }

        public BaseEntity() {
            Status = true;
            // Guardar CreatedAt sin Kind UTC para compatibilidad con "timestamp without time zone"
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        }
    }
}