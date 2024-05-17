using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace Ginnis.WebAPIs.Helpers
{
    public static class ConfirmEmailBody
    {
        public static string EmailStringBody(string email, string confirmToken, string otp)
        {
            return $@"<html>
            <head>
            </head>
            <body style=""margin:0; padding:0"">
                <div style=""height: auto; background: linear-gradient(to top, pink, white) no-repeat; display: flex; flex-direction: column;"">
                  
                    <div>
                        <h1>OTP Verification</h1>
                        <hr>
                        <p>Thank you for using our service. Your OTP for verification is: <strong>{otp}</strong></p>
                        <p>Please use this OTP to verify your email.</p>
                        <p>If you didn't request this OTP, please ignore this email.</p>
                        <p>Kind Regards,<br><br>Ginni</p>
                    </div>
                </div>
            </body>
            </html>";
        }

          //        <div>
          //              <h1>Confirm Your Email</h1>
          //              <hr>
          //              <p>Thank you for registering with us.Please click the button below to confirm your email.</p>
          //              <a href = ""http://localhost:4200/ginniconfirmemail?email={email}&token={confirmToken}"" target=""_blank"" style=""background:red; padding:10px; border:none; color:white; border-radius:4px; display:block; width:25%; text-align:center; text-decoration:none"">Confirm Email</a><br>
          //              <p>Kind Regards,<br><br>Ginni</p>
          //          </div>
    }
}
