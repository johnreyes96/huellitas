using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Data.Entities
{
    public class AppointmentType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de cita")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }
    }
}
