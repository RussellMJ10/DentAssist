using System;
using DentAssistProyect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DentAssistProyect.Models.Entities
{
    public class Turno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias")]
        public DateTime FechaHora { get; set; }

        [Range(15, 120, ErrorMessage = "La duración debe ser entre 15 y 120 minutos")]
        public int DuracionMinutos { get; set; } = 30;

        [Required]
        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

        public int? PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int? OdontologoId { get; set; }
        public Odontologo? Odontologo { get; set; }
    }
}
