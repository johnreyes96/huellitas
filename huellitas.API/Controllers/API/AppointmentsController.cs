using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Models.Request;
using huellitas.API.Helpers;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AppointmentsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            Appointment appointment = await _context.Appointments
                    .Include(x => x.AppointmentType)
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(request.UserId));
            if (user == null)
            {
                return BadRequest("No existe el usuario.");
            }

            AppointmentType appointmentType = await _context.AppointmentTypes.FindAsync(request.AppointmentTypeId);
            if (appointmentType != null)
            {
                return BadRequest("No existe el tipo de cita.");
            }

            if (request.Date < DateTime.Now)
            {
                return BadRequest("La fecha de la cita debe ser mayor o igual a la fecha actual.");
            }

            Appointment appointment = await _context.Appointments
                .Include(x => x.AppointmentType)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Date >= DateTime.UtcNow
                && x.AppointmentType.Id == appointmentType.Id
                && x.User.Id == user.Id);
            if (appointment != null)
            {
                return BadRequest("Ya tienes una " + appointment.AppointmentType.Description + " para el día " + appointment.Date+".");
            }

            List<Appointment> appointments = await _context.Appointments
                   .Include(x => x.AppointmentType)
                   .Include(x => x.User)
                   .Where(x => x.Date == request.Date.ToUniversalTime()
                   && x.AppointmentType.Id == appointmentType.Id)
                   .ToListAsync();
            if (appointments != null && appointments.Count >= 1)
            {
                return BadRequest("Ya no hay mas citas disponibles para la fecha y hora seleccionada.");
            }

            appointment = new Appointment()
            {
                AppointmentType = appointmentType,
                User = user,
                Date = request.Date
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return Ok(appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, AppointmentRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(request.UserId));
            if (user == null)
            {
                return BadRequest("No existe el usuario.");
            }

            AppointmentType appointmentType = await _context.AppointmentTypes.FindAsync(request.AppointmentTypeId);
            if (appointmentType != null)
            {
                return BadRequest("No existe el tipo de cita.");
            }

            if (request.Date < DateTime.Now)
            {
                return BadRequest("La fecha de la cita debe ser mayor o igual a la fecha actual.");
            }

            Appointment appointment = await _context.Appointments
                .Include(x => x.AppointmentType)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Date >= DateTime.UtcNow
                && x.AppointmentType.Id == appointmentType.Id
                && x.User.Id == user.Id);
            if (appointment != null)
            {
                return BadRequest("Ya tienes una " + appointment.AppointmentType.Description + " para el día " + appointment.Date + ".");
            }

            List<Appointment> appointments = await _context.Appointments
                   .Include(x => x.AppointmentType)
                   .Include(x => x.User)
                   .Where(x => x.Date == request.Date.ToUniversalTime()
                   && x.AppointmentType.Id == appointmentType.Id)
                   .ToListAsync();
            if (appointments != null && appointments.Count >= 1)
            {
                return BadRequest("Ya no hay mas citas disponibles para la fecha y hora seleccionada.");
            }

            appointment = await _context.Appointments.FindAsync(request.Id);
            if (appointment != null)
            {
                return BadRequest("No existe la cita.");
            }

            appointment.AppointmentType = appointmentType;
            appointment.Date = request.Date;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            Appointment appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
