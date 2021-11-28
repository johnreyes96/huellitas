using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class ServiceDetailRequest
    {
        public int Id { get; set; }

        [Display(Name = "Detalle factura")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BillingDetailId { get; set; }

        [Display(Name = "Descripción servicio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }
    }
}
