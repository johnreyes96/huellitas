using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Models.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ServiceDetailsController : ControllerBase
    {
        private readonly DataContext _context;

        public ServiceDetailsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDetail>> PostServiceDetail(ServiceDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BillingDetail billingDetail = await _context.BillingDetails.FindAsync(request.BillingDetailId);
            if (billingDetail == null)
            {
                return BadRequest("No existe el detalle de la factura.");
            }

            ServiceDetail serviceDetail = new()
            {
                billingDetail = billingDetail,
                Description = request.Description
            };

            _context.ServicesDetails.Add(serviceDetail);
            await _context.SaveChangesAsync();
            return Ok(serviceDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceDetail(int id, ServiceDetailRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BillingDetail billingDetail = await _context.BillingDetails.FindAsync(request.BillingDetailId);
            if (billingDetail == null)
            {
                return BadRequest("No existe el detalle de la factura.");
            }

            ServiceDetail serviceDetail = await _context.ServicesDetails.FindAsync(request.Id);
            if (serviceDetail == null)
            {
                return BadRequest("No existe el detalle del servicio.");
            }

            serviceDetail.Description = request.Description;

            _context.ServicesDetails.Update(serviceDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceDetail(int id)
        {
            ServiceDetail serviceDetail = await _context.ServicesDetails.FindAsync(id);
            if (serviceDetail == null)
            {
                return NotFound();
            }

            _context.ServicesDetails.Remove(serviceDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
