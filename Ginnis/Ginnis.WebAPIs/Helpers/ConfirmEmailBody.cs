namespace Ginnis.WebAPIs.Helpers
{
    public static class ConfirmEmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
            <head>
            </head>
            <body style=""margin:0; padding:0"">
                <div style=""height: auto; background: linear-gradient(to top, pink, white) no-repeat;"">
                    <div>
                        <h1>Confirm Your Email</h1>
                        <hr>
                        <p>Thank you for registering with us. Please click the button below to confirm your email.</p>
                        <a href=""http://localhost:4200/ginniconfirmemail?email={email}&token={emailToken}"" target=""_blank"" style=""background:red; padding:10px; border:none; color:white; border-radius:4px; display:block; width:25%; text-align:center; text-decoration:none"">Confirm Email</a><br>
                        <p>Kind Regards,<br><br>Ginni</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
