using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Helpers;
using huellitas.API.Models.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetPhotoesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;

        public PetPhotoesController(DataContext context, IBlobHelper blobHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
        }

        [HttpPost]
        public async Task<ActionResult<PetPhoto>> PostPetPhoto(PetPhotoRequest request)
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

            Guid imageId = await _blobHelper.UploadBlobAsync(request.Image, "petphotos");
            PetPhoto petPhoto = new()
            {
                ImageId = imageId,
                Pet = pet
            };

            _context.PetPhotos.Add(petPhoto);
            await _context.SaveChangesAsync();
            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetPhoto(int id)
        {
            PetPhoto petPhoto = await _context.PetPhotos.FindAsync(id);
            if (petPhoto == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(petPhoto.ImageId, "petphotos");
            _context.PetPhotos.Remove(petPhoto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
