using huellitas.Common.Models;

namespace huellitas.API.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
