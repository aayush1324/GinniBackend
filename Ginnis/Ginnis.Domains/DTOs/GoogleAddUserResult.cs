﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class GoogleAddUserResult
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public string Token { get; set; }
    }
}
