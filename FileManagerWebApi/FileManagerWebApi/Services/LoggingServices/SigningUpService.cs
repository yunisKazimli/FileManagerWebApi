using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public class SigningUpService : ISigningUpService
    {
        public void SignUp(SignUpModel signUpModel, IUserFileManagerService userFMService)
        {
            if (signUpModel.Password != signUpModel.ConfirmPassword) throw new Exception("password and confirmation password is not equal");

            if (userFMService.ReadAllUsers().IndexOf(signUpModel.Gmail) != -1) throw new Exception("this gmail already signed up");

            if (signUpModel.Gmail.IndexOf("@gmail.com") == -1) throw new Exception("it is not gmail");

            if (signUpModel.Password.Length < 6) throw new Exception("password too short");

            //SubmitConfirmCurl(signUpModel);

            userFMService.WriteNewUser(new UserModel(signUpModel.Gmail, StaticClasses.HashManager.GetHash(signUpModel.Password), true));
        }

        /*private void SubmitConfirmCurl(SignUpModel signUpModel)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("yunis.ivanov90@gmail.com");
                mail.To.Add(signUpModel.Gmail);
                mail.Subject = "Hello World";
                mail.Body = "<h1>Hello</h1>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(signUpModel.Gmail, "");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }*/
    }
}
