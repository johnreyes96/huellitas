using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace huellitas.API.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [Display(Name = "Tipo de cita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int AppointmentTypeId { get; set; }

        public IEnumerable<SelectListItem> AppointmentTypes { get; set; }
    }
}
