using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace huellitas.API.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Display(Name = "Dueño de mascota")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [Display(Name = "Tipo de cita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public AppointmentType AppointmentType { get; set; }
    }
}
