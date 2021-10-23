using huellitas.Common.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;

namespace huellitas.API.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response SendMail(string to, string subject, string body)
        {
            try
            {
                string from = _configuration["Mail:From"];
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string password = _configuration["Mail:Password"];
                MimeMessage message = new();
                message.From.Add(new MailboxAddress(from));
                message.To.Add(new MailboxAddress(to));
                message.Subject = subject;
                BodyBuilder bodyBuilder = new()
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();
                using (SmtpClient client = new())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return new Response { IsSuccess = true };
            }
            catch (Exception error)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = error.Message,
                    Result = error
                };
            }
        }
    }
}
