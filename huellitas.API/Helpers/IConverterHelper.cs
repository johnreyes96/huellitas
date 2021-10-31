using huellitas.API.Data.Entities;
using huellitas.API.Models;
using System;
using System.Threading.Tasks;

namespace huellitas.API.Helpers
{
    public interface IConverterHelper
    {
        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);

        UserViewModel ToUserViewModel(User user);

        Task<Pet> ToPetAsync(PetViewModel model, bool isNew);

        PetViewModel ToPetViewModel(Pet pet);


    }
}
