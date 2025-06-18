using DentAssistProyect.Models.Entities;
using DentAssistProyect.Models.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentAssistProyect.Models.Entities
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Rut(ErrorMessage = "El RUT no es válido")]
        public string Rut { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un email válido")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Direccion { get; set; }

        public ICollection<Turno> Turnos { get; set; }= new List<Turno>();
        public ICollection<PlanTratamiento> PlanesTratamiento { get; set; } = new List<PlanTratamiento>();

    }
}