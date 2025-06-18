using DentAssistProyect.Models.Entities;
using System;
using System.Collections.Generic;

namespace DentAssistProyect.Models.Entities
{
    public class PlanTratamiento
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Observaciones { get; set; }

        public int? PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int? OdontologoId { get; set; }
        public Odontologo? Odontologo { get; set; }

        public ICollection<PasoPlan> Pasos { get; set; } = new List<PasoPlan>();
    }
}
