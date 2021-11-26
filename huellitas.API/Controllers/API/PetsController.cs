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
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;

        public PetsController(DataContext context, IBlobHelper blobHelper, IUserHelper userHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            Pet pet = await _context.Pets
                .Include(x => x.petType)
                .Include(x => x.PetPhotos)
                .Include(x => x.Billings)
                .ThenInclude(x => x.BillingDetails)
                .ThenInclude(x => x.ServiceDetails)
                .Include(x => x.Billings)
                .ThenInclude(x => x.BillingDetails)
                .ThenInclude(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(PetRequest request)
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

            PetType petType = await _context.PetTypes.FindAsync(request.petTypeId);
            if (petType != null)
            {
                return BadRequest("No existe el tipo de mascota.");
            }

            List<PetPhoto> petPhotos = new();
            if (request.Image != null && request.Image.Length > 0)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(request.Image, "petphotos");
                petPhotos.Add(new PetPhoto
                {
                    ImageId = imageId
                });
            }

            Pet pet = new()
            {
                petType = petType,
                User = user,
                Name = request.Name,
                Race = request.Race,
                Color = request.Color,
                Observations = request.Observations,
                PetPhotos = petPhotos,
                Billings = new List<Billing>()
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return Ok(pet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(int id, PetRequest request)
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

            PetType petType = await _context.PetTypes.FindAsync(request.petTypeId);
            if (petType != null)
            {
                return BadRequest("No existe el tipo de mascota.");
            }

            Pet pet = await _context.Pets.FindAsync(request.Id);
            if (pet != null)
            {
                return BadRequest("No existe la mascota.");
            }

            pet.petType = petType;
            pet.Name = request.Name;
            pet.Race = request.Race;
            pet.Color = request.Color;
            pet.Observations = request.Observations;

            try
            {
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe esta mascota.");
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
        public async Task<IActionResult> DeletePet(int id)
        {
            Pet pet = await _context.Pets
                .Include(x => x.PetPhotos)
                .Include(x => x.Billings)
                .ThenInclude(x => x.BillingDetails)
                .ThenInclude(x => x.ServiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
