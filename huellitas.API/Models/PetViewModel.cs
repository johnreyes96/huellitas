using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Models
{
    public class PetViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Mascota")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de mascota.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PetTypeId { get; set; }

        public IEnumerable<SelectListItem> PetTypes { get; set; }

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
        [DataType(DataType.MultilineText)]
        public string Observations { get; set; }

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }
    }
}
