using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace huellitas.API.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [JsonIgnore]
        [Display(Name = "Tipo de Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public PetType petType { get; set; }

        [Display(Name = "Usuario")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

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

        [Display(Name = "# Fotos")]
        public int PetPhotosCount => PetPhotos == null ? 0 : PetPhotos.Count;

        public ICollection<PetPhoto> PetPhotos { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => PetPhotos == null || PetPhotos.Count == 0
           ? $"https://apihuellitas.azurewebsites.net/images/no_image.png"
           : PetPhotos.FirstOrDefault().ImageFullPath;

        public ICollection<Billing> Billings { get; set; }

        [Display(Name = "# Facturas")]
        public int BillingsCount => Billings == null ? 0 : Billings.Count;
    }
}
