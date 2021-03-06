using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace huellitas.API.Data.Entities
{
    public class BillingDetail
    {
        public int Id { get; set; }

        [Display(Name = "Factura")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Billing Billing { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Service Service { get; set; }

        [Display(Name = "Valor Unitario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UnitValue { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }

        [Display(Name = "Valor Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public int ValueSubtotal => UnitValue * Quantity;

        public ICollection<ServiceDetail> ServiceDetails { get; set; }
    }
}
