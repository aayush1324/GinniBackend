using Ginnis.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface ITwilioRepository
    {
        Task<bool> AddVerificationDataAsync(TwilioVerify verificationData);
        Task<TwilioVerify> GetVerificationDataAsync(string mobileNumber);
        Task<bool> VerifyOtpAsync(string mobileNumber, string verificationCode);

    }
}
