using Ginnis.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Ginnis.Repos.Repositories
{
    public class TwilioSmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public TwilioSmsService(string accountSid, string authToken, string fromPhoneNumber)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromPhoneNumber = fromPhoneNumber;
        }

        public Task SendSmsAsync(string toPhoneNumber, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(toPhoneNumber))
            {
                From = new PhoneNumber(_fromPhoneNumber),
                Body = message
            };

            var msg = new MessageResource.Create(messageOptions);

            return Task.FromResult(msg);
        }

        
    }
}
