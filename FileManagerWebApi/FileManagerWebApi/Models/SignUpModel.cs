using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Models
{
    public class SignUpModel
    {
        public string Gmail { get; set; }

        public string Password { get; set; }

        /*public string ConfirmPassword { get; set; }*/

        public SignUpModel(string gmail, string password/*, string confirmPassword*/)
        {
            Gmail = gmail;
            Password = password;
            /*ConfirmPassword = confirmPassword;*/
        }
    }
}
