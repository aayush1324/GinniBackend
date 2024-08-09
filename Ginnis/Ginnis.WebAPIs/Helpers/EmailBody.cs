namespace Ginnis.WebAPIs.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>

            <head>
            </head>
            <body style = ""margin:0; padding:0"">
                <div style = ""height: auto; background: linear-gradient(to top, pink, white) no-repeat;"">
                    <div>
                        <h1> Reset your Password</h1>
                        <hr>
                        <p> You're receiving this e-mail because you requested a password reset for your Ginni.</p>
                        <p> Please tap the button below to choose a new password</p>

                        <a href=""http://localhost:4200/main/ginniresetpassword?email={email}&code={emailToken}"" target=""_blank""  
                            style = ""background:red; padding:10px; border:none; color:white; border-radius:4px;
                            display:block; width:25%; text-align:center; text-decoration:none"">Reset Password </a><br>
                    </div>
                </div>
            </body>
            </html> ";
        }
    }
}
