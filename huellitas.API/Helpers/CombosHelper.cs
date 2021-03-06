using huellitas.API.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace huellitas.API.Helpers
{

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboAppointmentTypes()
        {

            List<SelectListItem> list = _context.AppointmentTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            }).OrderBy(x => x.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de cita...]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboDocumentTypes()
        {
        
        List<SelectListItem> list = _context.DocumentTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            }).OrderBy(x => x.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de documento...]",
                Value = "0"
            });
            return list;
        }


        public IEnumerable<SelectListItem> GetComboPetTypes()
        {

            List<SelectListItem> list = _context.PetTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            }).OrderBy(x => x.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de mascota...]",
                Value = "0"
            });
            return list;
        }



        public IEnumerable<SelectListItem> GetComboServices()
        {

            List<SelectListItem> list = _context.Services.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = $"{x.Id}"
            }).OrderBy(x => x.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un servicio...]",
                Value = "0"
            });
            return list;
        }



      

    }
}
