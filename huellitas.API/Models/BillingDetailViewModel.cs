using huellitas.API.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Models
{
    public class BillingDetailViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Precio servicio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UnitValue { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Quantity { get; set; }


        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public int BillingId { get; set; }


        [Display(Name = "Detalle Servicio")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public  ServiceDetail serviceDetail { get; set; }

        [Display(Name = "Servicio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un servicio.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ServiceId { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }

    }
}
