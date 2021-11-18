using huellitas.API.Data;
using huellitas.API.Data.Entities;
using huellitas.API.Helpers;
using huellitas.API.Models;
using huellitas.Common.Enums;
using huellitas.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace huellitas.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;

        public UsersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper,
            IConverterHelper converterHelper, IBlobHelper blobHelper, IMailHelper mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.pets)
                .Where(x => x.UserType == UserType.User)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            UserViewModel model = new()
            {
                DocumentTypes = _combosHelper.GetComboDocumentTypes()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);
                user.UserType = UserType.User;
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Email, "Huellitas - Confirmación de cuenta", $"<h1>Huellitas - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");
                return RedirectToAction(nameof(Index));
            }
            model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel model = _converterHelper.ToUserViewModel(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, false);
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
            await _userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Pets(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.pets)
                .ThenInclude(x => x.petType )
                .Include(x => x.pets)
                .ThenInclude(x => x.PetPhotos)
                .Include(x => x.pets)
                .ThenInclude(x => x.Billings)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> AddPet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.pets)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            PetViewModel model = new PetViewModel
            {
                PetTypes = _combosHelper.GetComboPetTypes(),
                UserId = user.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel petViewModel)
        {
            User user = await _context.Users
                .Include(x => x.pets)
                .FirstOrDefaultAsync(x => x.Id == petViewModel.UserId);
            if (user == null)
            {
                return NotFound();
            }

            Guid imageId = Guid.Empty;
            if (petViewModel.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(petViewModel.ImageFile, "petphotos");
            }

            Pet pet = await _converterHelper.ToPetAsync(petViewModel, true);
            if (pet.PetPhotos == null)
            {
                pet.PetPhotos = new List<PetPhoto>();
            }

            pet.PetPhotos.Add(new PetPhoto
            {
                ImageId = imageId
            });

            try
            {
                user.pets.Add(pet);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Pets), new { id = user.Id });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe esta mascota.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            petViewModel.PetTypes = _combosHelper.GetComboPetTypes();
            return View(petViewModel);
        }

        public async Task<IActionResult> EditPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.petType)
                .Include(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetViewModel model = _converterHelper.ToPetViewModel(pet);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int id, PetViewModel petViewModel)
        {
            if (id != petViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Pet pet = await _converterHelper.ToPetAsync(petViewModel, false);
                    _context.Pets.Update(pet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Pets), new { id = petViewModel.UserId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una mascota con este nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }


            petViewModel.PetTypes = _combosHelper.GetComboPetTypes();
            return View(petViewModel);
        }


        public async Task<IActionResult> DeletePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Pets), new { id = pet.User.Id });
        }

        public async Task<IActionResult> DeleteImagePet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PetPhoto petPhoto = await _context.PetPhotos
                .Include(x => x.Pet)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (petPhoto == null)
            {
                return NotFound();
            }

            try
            {
                await _blobHelper.DeleteBlobAsync(petPhoto.ImageId, "petphotos");
            }
            catch { }

            _context.PetPhotos.Remove(petPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditPet), new { id = petPhoto.Pet.Id });
        }

        public async Task<IActionResult> AddPetImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            PetPhotoViewModel model = new()
            {
                PetId = pet.Id
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPetImage(PetPhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "petphotos");
                Pet pet = await _context.Pets
                    .Include(x => x.PetPhotos)
                    .FirstOrDefaultAsync(x => x.Id == model.PetId);
                if (pet.PetPhotos == null)
                {
                    pet.PetPhotos = new List<PetPhoto>();
                }

                pet.PetPhotos.Add(new PetPhoto
                {
                    ImageId = imageId
                });

                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditPet), new { id = pet.Id });
            }

            return View(model);

        }

        public async Task<IActionResult> Appointments(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.DocumentType)
                .Include(x => x.pets)
                .ThenInclude(x => x.petType)
                .Include(x => x.Appointments)
                .ThenInclude(x => x.AppointmentType)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> AddAppointment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .Include(x => x.Appointments)
                .ThenInclude(x => x.AppointmentType)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            AppointmentViewModel model = new AppointmentViewModel
            {
                AppointmentTypes = _combosHelper.GetComboAppointmentTypes(),
                Date = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm")),
                UserId = user.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAppointment(AppointmentViewModel appointmentViewModel)
        {
            User user = await _context.Users
            .Include(x => x.Appointments)
            .ThenInclude(x => x.AppointmentType)
            .FirstOrDefaultAsync(x => x.Id == appointmentViewModel.UserId);
            if (user == null)
            {
                return NotFound();
            }

            if (appointmentViewModel.Date < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "La fecha de la cita debe ser mayor o igual a la fecha actual");
            } 
            else
            {
                Appointment appointment = await _context.Appointments
                .Include(x => x.AppointmentType)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Date >= DateTime.UtcNow
                && x.AppointmentType.Id == appointmentViewModel.AppointmentTypeId
                && x.User.Id == appointmentViewModel.UserId);
                if (appointment != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya tienes una " + appointment.AppointmentType.Description + " para el día " + appointment.Date);
                }
                else
                {
                    List<Appointment> appointments = await _context.Appointments
                   .Include(x => x.AppointmentType)
                   .Include(x => x.User)
                   .Where(x => x.Date == appointmentViewModel.Date.ToUniversalTime()
                   && x.AppointmentType.Id == appointmentViewModel.AppointmentTypeId)
                   .ToListAsync();
                    if (appointments != null && appointments.Count >= 1)
                    {
                        ModelState.AddModelError(string.Empty, "Ya no hay mas citas disponibles para la fecha y hora seleccionada");
                    } 
                    else
                    {
                        appointment = await _converterHelper.ToAppointmentAsync(appointmentViewModel, true);

                        try
                        {
                            user.Appointments.Add(appointment);
                            _context.Users.Update(user);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Appointments), new { id = user.Id });
                        }
                        catch (Exception exception)
                        {
                            ModelState.AddModelError(string.Empty, exception.Message);
                        }
                    }

                }
            }

            appointmentViewModel.AppointmentTypes = _combosHelper.GetComboAppointmentTypes();
            return View(appointmentViewModel);
        }

        public async Task<IActionResult> EditAppointment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Appointment appointment = await _context.Appointments
                .Include(x => x.AppointmentType)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            AppointmentViewModel model = _converterHelper.ToAppointmentViewModel(appointment);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(int id, AppointmentViewModel appointmentViewModel)
        {
            if (id != appointmentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (appointmentViewModel.Date < DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "La fecha de la cita debe ser mayor o igual a la fecha actual");
                }
                else
                {
                    Appointment appointment = await _context.Appointments
                    .Include(x => x.AppointmentType)
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Date >= DateTime.UtcNow
                    && x.AppointmentType.Id == appointmentViewModel.AppointmentTypeId
                    && x.Id != appointmentViewModel.Id
                    && x.User.Id == appointmentViewModel.UserId);
                    if (appointment != null)
                        {
                            ModelState.AddModelError(string.Empty, "Ya tienes una " + appointment.AppointmentType.Description + " para el día " + appointment.Date);
                        }
                        else
                        {
                            List<Appointment> appointments = await _context.Appointments
                           .Include(x => x.AppointmentType)
                           .Include(x => x.User)
                           .Where(x => x.Date == appointmentViewModel.Date.ToUniversalTime()
                           && x.AppointmentType.Id == appointmentViewModel.AppointmentTypeId)
                           .ToListAsync();
                            if (appointments != null && appointments.Count >= 1)
                            {
                                ModelState.AddModelError(string.Empty, "Ya no hay mas citas disponibles para la fecha y hora seleccionada");
                            }
                            else
                            {
                                try
                                {
                                    appointment = await _converterHelper.ToAppointmentAsync(appointmentViewModel, false);
                                    _context.Appointments.Update(appointment);
                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Appointments), new { id = appointmentViewModel.UserId });
                                }
                                catch (Exception exception)
                                {
                                    ModelState.AddModelError(string.Empty, exception.Message);
                                }
                            }
                        }
                }
            }

            appointmentViewModel.AppointmentTypes = _combosHelper.GetComboAppointmentTypes();
            return View(appointmentViewModel);
        }

        public async Task<IActionResult> DeleteAppointment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Appointment appointment = await _context.Appointments
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Appointments), new { id = appointment.User.Id });
        }


        public async Task<IActionResult> BillingPet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets
                .Include(x => x.User)
                .Include(x => x.petType)
                .Include(x => x.Billings)
                .ThenInclude(x => x.BillingDetails)
                .Include(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBilling(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            BillingViewModel model = new BillingViewModel
            {
                PetId = pet.Id
                
            };

            
                Pet pets = await _context.Pets
                    .Include(x => x.Billings)
                    .FirstOrDefaultAsync(x => x.Id == model.PetId);
                if (pets == null)
                {
                    return NotFound();
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                Billing billing = new Billing
                {
                    Date = DateTime.UtcNow,
                    User = user
                };

                if (pet.Billings == null)
                {
                    pet.Billings = new List<Billing>();
                }

                pet.Billings.Add(billing);
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BillingPet), new { id = pet.Id });
            
        }




        public async Task<IActionResult> BillingDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Billing billing = await _context.Billings
                .Include(x => x.BillingDetails)
                .ThenInclude(x => x.Service)
                .Include(x => x.Pet)
                .ThenInclude(x => x.PetPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        public async Task<IActionResult> AddBillingDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Billing billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            BillingDetailViewModel model = new BillingDetailViewModel
            {
                BillingId = billing.Id,
                Services = _combosHelper.GetComboServices()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillingDetail(BillingDetailViewModel billingDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                Billing billing = await _context.Billings
                    .Include(x => x.BillingDetails)
                    .FirstOrDefaultAsync(x => x.Id == billingDetailViewModel.BillingId);
                if (billing == null)
                {
                    return NotFound();
                }

                if (billing.BillingDetails == null)
                {
                    billing.BillingDetails = new List<BillingDetail>();
                }

                BillingDetail detailBilling = await _converterHelper.ToBillingDetailAsync(billingDetailViewModel, true);
                billing.BillingDetails.Add(detailBilling);
                _context.Billings.Update(billing);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BillingDetails), new { id = billingDetailViewModel.BillingId});
            }

            billingDetailViewModel.Services = _combosHelper.GetComboServices();
            return View(billingDetailViewModel);
        }


    }
}
