using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class PetRequest
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int petTypeId { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserId { get; set; }

        [Display(Name = "Nombre Mascota")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Raza")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Race { get; set; }

        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

        [Display(Name = "Observaciones")]
        public string Observations { get; set; }

        public byte[] Image { get; set; }
    }
}
