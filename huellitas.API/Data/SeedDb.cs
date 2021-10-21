using huellitas.API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckDocumentTypesAsync();
            await CheckPetTypesAsync();
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
    }
}
