namespace Ginnis.WebAPIs.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
            <head>
            </head>
            <body style=""margin:0; padding:0px"">
                <div style=""margin:0; padding:50px 100px; height: auto; background: linear-gradient(179.7deg, rgb(197, 214, 227) 2.9%, rgb(144, 175, 202) 97.1%);"">
                    <div>

                        <img src=""https://ajfan.store/cdn/shop/files/golden_logo.png?v=1714450469&width=90"" alt=""Ginni DryFruits"" 
                            style=""width: 150px; height: 100px; display: block; margin-bottom: 20px;"">

                        <h1> Reset your Password</h1>
                        <p> Follow this link to reset your customer account password at 
                        <a href=""http://localhost:4200"" target=""_blank"" style=""color:#0066b2;"">Ginni DryFruits</a>.
                        If you didn't request a new password, you can safely delete this email.</p>
                        
                        <p>Please tap the button below to choose a new password:</p>

                        <div style=""display: flex; align-items: center; justify-content: center;"">
                            <a href=""http://localhost:4200/account/reset-password?email={email}&code={emailToken}"" target=""_blank""  
                                style=""background:#0066b2; padding:10px; border:none; color:white; border-radius:5px; 
                                text-align:center; text-decoration:none; margin-right:10px; display:inline-block;"">
                                Reset Your Password
                            </a>
                            <p> or <a href=""http://localhost:4200"" target=""_blank"" style="" margin-left:10px; color:#0066b2;"">Visit our Store</a></p>
                        </div>

                        <br>
                        <hr>
                        <p>If you have any questions, reply to this email or contact us at admin@ginnidryfruits.com.</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
