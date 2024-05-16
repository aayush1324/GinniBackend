using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class TwilioRepository : ITwilioRepository
    {
        private readonly AppDbContext _authContext;

        public TwilioRepository(AppDbContext authContext)
        {
            _authContext = authContext;
        }



        public async Task<bool> AddVerificationDataAsync(TwilioVerify verificationData)
        {
            _authContext.TwilioVerifys.Add(verificationData);
            await _authContext.SaveChangesAsync();
            return true;
        }




        public async Task<TwilioVerify> GetVerificationDataAsync(string mobileNumber)
        {
            return await _authContext.TwilioVerifys.SingleOrDefaultAsync(v => v.MobileNumber == mobileNumber);
        }




        public async Task<bool> VerifyOtpAsync(string mobileNumber, string verificationCode)
        {
            var otpVerificationData = await GetVerificationDataAsync(mobileNumber);

            if (otpVerificationData == null)
                return false;

            if (otpVerificationData.VerificationCode != verificationCode)
                return false;

            otpVerificationData.VerificationCode = null;
            otpVerificationData.Created_at  = DateTime.UtcNow;
            await _authContext.SaveChangesAsync();

            return true;
        }
    }

}
