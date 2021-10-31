using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace huellitas.API.Data.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de documento")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
