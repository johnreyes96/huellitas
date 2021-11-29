using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Helpers;
using huellitas.API.Models.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BillingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public BillingsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBilling(int id)
        {
            Billing billing = await _context.Billings
                .Include(x => x.BillingDetails)
                .ThenInclude(x => x.Service)
                .Include(x => x.BillingDetails)
                .ThenInclude(x => x.ServiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (billing == null)
            {
                return NotFound();
            }

            return billing;
        }

        [HttpPost]
        public async Task<ActionResult<Billing>> PostBilling(BillingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pet pet = await _context.Pets.FindAsync(request.PetId);
            if (pet == null)
            {
                return BadRequest("No existe la mascota.");
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return BadRequest("No existe el usuario.");
            }

            Billing billing = new()
            {
                Date = DateTime.UtcNow,
                BillingDetails = new List<BillingDetail>(),
                User = user,
                Pet = pet
            };

            _context.Billings.Add(billing);
            await _context.SaveChangesAsync();
            return Ok(billing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            Billing billing = await _context.Billings
                .Include(x => x.BillingDetails)
                .ThenInclude(x => x.ServiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

            _context.Billings.Remove(billing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
