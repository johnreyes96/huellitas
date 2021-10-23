using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace huellitas.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDocumentTypes();
    }
}
