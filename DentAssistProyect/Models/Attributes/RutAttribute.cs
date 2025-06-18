using System.ComponentModel.DataAnnotations;

namespace DentAssistProyect.Models.Attributes
{
    public class RutAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string rut = value.ToString().ToUpper().Replace(".", "").Replace("-", "");
            string rutBody = rut[0..^1];
            string dv = rut[^1..];

            if (!int.TryParse(rutBody, out _) || dv != "K" && !int.TryParse(dv, out _))
            {
                return new ValidationResult("El RUT ingresado no es válido");
            }

            return ValidationResult.Success;
        }
    }
}
