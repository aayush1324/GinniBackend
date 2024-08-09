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
    public class EmailRepo : IEmailRepo
    {
        private readonly IConfiguration _configuration;

        public EmailRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(Email email)
        {
            var emailMessage = new MimeMessage();

            var from = _configuration["EmailSettings:From"];

            emailMessage.From.Add(new MailboxAddress("Ginni DryDruits", from));
            emailMessage.To.Add(new MailboxAddress(email.To, email.To));

            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content)
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
