using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Helpers;
using huellitas.API.Models.Request;
using huellitas.Common.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;

        public UsersController(DataContext context, IUserHelper userHelper, IMailHelper mailHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(x => x.DocumentType)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Where(x => x.UserType == UserType.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            User user = await _context.Users
                .Include(x => x.DocumentType)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DocumentType documentType = await _context.DocumentTypes.FindAsync(request.DocumentTypeId);
            if (documentType == null)
            {
                return BadRequest("El tipo de documento no existe.");
            }
            User user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest("Ya existe un usuario registrado con ese email.");
            }
            Guid imageId = Guid.Empty;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "users");
            }
            user = new User
            {
                Address = request.Address,
                Document = request.Document,
                DocumentType = documentType,
                Email = request.Email,
                FirstName = request.FirstName,
                ImageId = imageId,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                UserType = UserType.User,
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, "Huellitas - Confirmación de cuenta", $"<h1>Huellitas - Confirmación de cuenta</h1>" +
                $"Para habilitar el usuario, " +
                $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");
            return Ok(user);
        }
    }
}
