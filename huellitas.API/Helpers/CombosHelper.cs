using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace huellitas.API.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public IEnumerable<SelectListItem> GetComboDocumentTypes()
        {
            List<SelectListItem> list = new(); //TODO: @Diana realizar el llenado del combo document type
            return list;
        }
    }
}
