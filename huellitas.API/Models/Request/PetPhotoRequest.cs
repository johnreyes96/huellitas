using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class PetPhotoRequest
    {
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
