using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace huellitas.API.Data.Entities
{
    public class PetPhoto
    {
        public int Id { get; set; }


        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet Pet { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }


        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://huellitasapi.azurewebsites.net/images/no_image.png"
            : $"https://huellitas.blob.core.windows.net/petphotos/{ImageId}";
    }
}
