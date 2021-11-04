using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace huellitas.API.Data.Entities
{
    public class Billing
    {
        public int Id { get; set; }

        [Display(Name = "Veterinario")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        [Display(Name = "Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet Pet { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        
        [Display(Name = "Valor Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public int TotalValue => BillingDetails == null ? 0 : BillingDetails.Sum(x => (x.ValueSubtotal*19/100) + x.ValueSubtotal);

        public ICollection<BillingDetail> BillingDetails { get; set; }

        [Display(Name = "# Servicios")]
        public int BillingDetailsCount => BillingDetails == null ? 0 : BillingDetails.Count;



    }
}
