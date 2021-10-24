using huellitas.API.Data.Entities;
using huellitas.API.Helpers;
using huellitas.Common.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypesAsync();
            await CheckPetTypesAsync();
            await CheckServicesAsync();
            /*await CheckRolesAsync();
            await CheckUserAsync("1037965896", "Pedro", "Salazar", "pedro@yopmail.com", "3004568596", "Calle 10 # 10 10", UserType.Admin);
            await CheckUserAsync("1035456218", "Claudia", "Mendez", "claudia@yopmail.com", "3165489650", "Autopista sur # 40 160", UserType.Admin);
            await CheckUserAsync("1027456586", "Rodrigo", "Rodriguez", "rodrigo@yopmail.com", "3102568459", "Carrera 65 # 65 65", UserType.User);
            await CheckUserAsync("1034459542", "Lucia", "Torres", "lucia@yopmail.com", "3152156548", "Transversal 7 # 10 10", UserType.User);*/
        }

        private async Task CheckDocumentTypesAsync()
        {
            if (!_context.DocumentTypes.Any())
            {
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula de Ciudadanía" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Cédula de Extranjería" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Tarjeta de Identidad" });
                _context.DocumentTypes.Add(new DocumentType { Description = "NIT" });
                _context.DocumentTypes.Add(new DocumentType { Description = "Pasaporte" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetTypesAsync()
        {
            if (!_context.PetTypes.Any())
            {
                _context.PetTypes.Add(new PetType { Description = "Burro" });
                _context.PetTypes.Add(new PetType { Description = "Caballo" });
                _context.PetTypes.Add(new PetType { Description = "Conejo" });
                _context.PetTypes.Add(new PetType { Description = "Gato" });
                _context.PetTypes.Add(new PetType { Description = "Hamnster" });
                _context.PetTypes.Add(new PetType { Description = "Loro" });
                _context.PetTypes.Add(new PetType { Description = "Oveja" });
                _context.PetTypes.Add(new PetType { Description = "Pato" });
                _context.PetTypes.Add(new PetType { Description = "Perro" });
                _context.PetTypes.Add(new PetType { Description = "Vaca" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckServicesAsync()
        {
            if (!_context.Services.Any())
            {
                _context.Services.Add(new Service { Description = "Cirugía" });
                _context.Services.Add(new Service { Description = "Esterilización" });
                _context.Services.Add(new Service { Description = "Farmacología" });
                _context.Services.Add(new Service { Description = "Guardería" });
                _context.Services.Add(new Service { Description = "Hospitalización" });
                _context.Services.Add(new Service { Description = "Imágenes diagnósticas" });
                _context.Services.Add(new Service { Description = "Laboratorio clínico" });
                _context.Services.Add(new Service { Description = "Peluquería" });
                _context.Services.Add(new Service { Description = "Tienda para mascotas" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckUserAsync(string document, string firstName, string lastName, string email, string phoneNumber,
            string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Document = document,
                    DocumentType = _context.DocumentTypes.FirstOrDefault(x => x.Description == "Cédula"),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }
    }
}
