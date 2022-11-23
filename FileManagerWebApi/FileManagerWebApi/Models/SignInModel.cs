using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Models
{
    public class SignInModel
    {
        public string Gmail { get; }

        public string Password { get; }

        public SignInModel(string gmail, string password)
        {
            Gmail = gmail;
            Password = password;
        }
    }
}
