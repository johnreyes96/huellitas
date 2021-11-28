using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class BillingDetailRequest
    {
        public int Id { get; set; }

        [Display(Name = "Factura")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BillingId { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ServiceId { get; set; }

        [Display(Name = "Valor unitario")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UnitValue { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }
    }
}
