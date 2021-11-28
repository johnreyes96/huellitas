using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class BillingRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PetId { get; set; }
    }
}
