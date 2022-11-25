using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public class SigningUpService : ISigningUpService
    {
        private readonly string mainDirectoryPath = @"D:\Projects\VSprojects\FileManagerWebApi\UserFilesFolder";

        public string[] GetAllGmails()
        {
            return CutGmail(Directory.GetDirectories(mainDirectoryPath).Where(x => x.IndexOf('@') != -1).ToArray());
        }

        private string[] CutGmail(string[] allGmails)
        {
            for(int i = 0; i < allGmails.Length; i++)
            {
                int index = allGmails[i].IndexOf("@gmail.com");

                for (int j = index; j >= 0; j--)
                {
                    if (allGmails[i][j - 1] == '\\')
                    {
                        index = j;

                        break;
                    }
                }

                allGmails[i] = allGmails[i].Substring(index, allGmails[i].Length - index);    
            }

            return allGmails;
        }

        public void SignUp(SignUpModel signUpModel, IUserFileManagerService userFMService)
        {
            /*if (signUpModel.Password != signUpModel.ConfirmPassword) throw new Exception("password and confirmation password is not equal");*/

            if (userFMService.ReadAllUsers().IndexOf(signUpModel.Gmail) != -1) throw new Exception("this gmail already signed up");

            if (signUpModel.Gmail.IndexOf("@gmail.com") == -1) throw new Exception("it is not gmail");

            if (!Regex.IsMatch(signUpModel.Gmail, @"^[\w.+\-]+@gmail\.com$")) throw new Exception("you used forbidden characters");

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
