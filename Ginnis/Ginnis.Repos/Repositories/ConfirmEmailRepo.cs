using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class ConfirmEmailRepo : IConfirmEmailRepo
    {
        private readonly IConfiguration _configuration;

        public ConfirmEmailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendConfirmEmail(ConfirmEmail confirmEmail)
        {
            var emailMessage = new MimeMessage();

            var from = _configuration["EmailSettings:From"];

            emailMessage.From.Add(new MailboxAddress("Ginni Dry Fruits", from));
            emailMessage.To.Add(new MailboxAddress(confirmEmail.To, confirmEmail.To));

            emailMessage.Subject = confirmEmail.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(confirmEmail.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_configuration["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_configuration["EmailSettings:From"], _configuration["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }

                catch (Exception ex)
                {
                    throw;
                }

                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
