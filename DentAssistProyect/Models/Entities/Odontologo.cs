using System.ComponentModel.DataAnnotations;


namespace DentAssistProyect.Models.Entities
{
    public class Odontologo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La matrícula es obligatoria")]
        public string Matricula { get; set; }

        public string Especialidad { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un email válido")]
        public string Email { get; set; }

        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
        public ICollection<PlanTratamiento> PlanesTratamiento { get; set; } = new List<PlanTratamiento>();
    }
}
