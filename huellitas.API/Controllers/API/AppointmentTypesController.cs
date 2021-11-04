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

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AppointmentTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public AppointmentTypesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentType>>> GetAppointmentTypes()
        {
            return await _context.AppointmentTypes.OrderBy(x => x.Description).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentType>> GetAppointmentType(int id)
        {
            AppointmentType appointmentType = await _context.AppointmentTypes.FindAsync(id);

            if (appointmentType == null)
            {
                return NotFound();
            }

            return appointmentType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentType(int id, AppointmentType appointmentType)
        {
            if (id != appointmentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("El tipo de cita ya existe.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentType>> PostAppointmentType(AppointmentType appointmentType)
        {
            _context.AppointmentTypes.Add(appointmentType);

            try
            {
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAppointmentType", new { id = appointmentType.Id }, appointmentType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("El tipo de cita ya existe.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentType(int id)
        {
            AppointmentType appointmentType = await _context.AppointmentTypes.FindAsync(id);

            if (appointmentType == null)
            {
                return NotFound();
            }

            _context.AppointmentTypes.Remove(appointmentType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
