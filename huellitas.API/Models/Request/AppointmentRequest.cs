using System;
using System.ComponentModel.DataAnnotations;

namespace huellitas.API.Models.Request
{
    public class AppointmentRequest
    {
        public int Id { get; set; }

        [Display(Name = "Dueño de mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserId { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [Display(Name = "Tipo de cita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int AppointmentTypeId { get; set; }
    }
}
