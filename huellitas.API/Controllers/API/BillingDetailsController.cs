using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Models.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BillingDetailsController : ControllerBase
    {
        private readonly DataContext _context;

        public BillingDetailsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BillingDetail>> GetBillingDetail(int id)
        {
            BillingDetail billingDetail = await _context.BillingDetails
                .Include(x => x.Service)
                .Include(x => x.ServiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (billingDetail == null)
            {
                return NotFound();
            }

            return billingDetail;
        }

        [HttpPost]
        public async Task<ActionResult<BillingDetail>> PostBillingDetail(BillingDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Billing billing = await _context.Billings.FindAsync(request.BillingId);
            if (billing == null)
            {
                return BadRequest("No existe la factura.");
            }

            Service service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
            {
                return BadRequest("No existe el servicio.");
            }

            BillingDetail billingDetail = new()
            {
                Billing = billing,
                Service = service,
                UnitValue = request.UnitValue,
                Quantity = request.Quantity
            };

            _context.BillingDetails.Add(billingDetail);
            await _context.SaveChangesAsync();
            return Ok(billingDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillingDetail(int id, BillingDetailRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Billing billing = await _context.Billings.FindAsync(request.BillingId);
            if (billing == null)
            {
                return BadRequest("No existe la factura.");
            }

            Service service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
            {
                return BadRequest("No existe el servicio.");
            }

            BillingDetail billingDetail = await _context.BillingDetails.FindAsync(request.Id);
            if (billingDetail == null)
            {
                return BadRequest("No existe el detalle de la factura.");
            }

            billingDetail.Service = service;
            billingDetail.UnitValue = request.UnitValue;
            billingDetail.Quantity = request.Quantity;

            _context.BillingDetails.Update(billingDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillingDetail(int id)
        {
            BillingDetail billingDetail = await _context.BillingDetails
                .Include(x => x.ServiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (billingDetail == null)
            {
                return NotFound();
            }

            _context.BillingDetails.Remove(billingDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
