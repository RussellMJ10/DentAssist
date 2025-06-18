using System.ComponentModel.DataAnnotations;

namespace DentAssistProyect.Models.Entities
{
    public class Tratamiento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser positivo")]
        public decimal PrecioEstimado { get; set; }
    }
}
