using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Models;
using System;
using System.Threading.Tasks;

namespace huellitas.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<Appointment> ToAppointmentAsync(AppointmentViewModel model, bool isNew)
        {
            return new Appointment
            {
                Id = isNew ? 0 : model.Id,
                Date = model.Date.ToUniversalTime(),
                AppointmentType = await _context.AppointmentTypes.FindAsync(model.AppointmentTypeId),
                User = await _context.Users.FindAsync(model.UserId)
            };
        }

        public AppointmentViewModel ToAppointmentViewModel(Appointment appointment)
        {
            return new AppointmentViewModel
            {
                Date = appointment.Date,
                AppointmentTypes = _combosHelper.GetComboAppointmentTypes(),
                AppointmentTypeId = appointment.AppointmentType.Id,
                Id = appointment.Id,
                UserId = appointment.User.Id
            };
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Document = model.Document,
                DocumentType = await _context.DocumentTypes.FindAsync(model.DocumentTypeId),
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType
            };
        }

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                DocumentTypeId = user.DocumentType.Id,
                DocumentTypes = _combosHelper.GetComboDocumentTypes(),
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType
            };
        }


        public async Task<Pet> ToPetAsync(PetViewModel model, bool isNew)
        {
            return new Pet
            {
                petType = await _context.PetTypes.FindAsync(model.PetTypeId),
                Color = model.Color,
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Observations = model.Observations,
                Race = model.Race.ToUpper(),
            };
        }


        

        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Color = pet.Color,
                Id = pet.Id,
                Name = pet.Name,
                Race = pet.Race,
                PetTypeId = pet.petType.Id,
                Observations = pet.Observations,
                PetPhotos = pet.PetPhotos,
                PetTypes = _combosHelper.GetComboPetTypes(),
                UserId = pet.User.Id
            };
        }
    }
}
