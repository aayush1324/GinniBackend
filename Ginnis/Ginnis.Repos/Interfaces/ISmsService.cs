﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync (string toPhoneNumber, string message);
    }
}
