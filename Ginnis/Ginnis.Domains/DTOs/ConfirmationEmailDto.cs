using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class ConfirmationEmailDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
        
        public int Port { get; set; }

        public string From { get; set; }

        public string SmtpServer { get; set; }
    }
}
