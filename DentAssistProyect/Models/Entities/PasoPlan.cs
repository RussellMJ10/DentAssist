using System;
using System.ComponentModel.DataAnnotations;
using DentAssistProyect.Models.Enums;

namespace DentAssistProyect.Models.Entities
{
    public class PasoPlan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El orden es requerido")]
        public int Orden { get; set; }

        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaEstimada { get; set; }

        [Required]
        public EstadoPaso Estado { get; set; } = EstadoPaso.Pendiente;

        public int? PlanTratamientoId { get; set; }
        public PlanTratamiento? PlanTratamiento { get; set; }

        public int? TratamientoId { get; set; }
        public Tratamiento? Tratamiento { get; set; }
    }
}
