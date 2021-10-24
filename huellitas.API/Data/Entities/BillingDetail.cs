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

        [Display(Name = "Numero Factura")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Billing Billing { get; set; }

        [Display(Name = "Valor Unitario")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Double UnitValue { get; set; }

        [Display(Name = "Cantidad")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }
    }
}
