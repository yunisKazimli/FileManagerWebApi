using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Models
{
    public class UserModel
    {
        public string Gmail { get; }

        public string Password { get; }

        public bool IsActive { get; }

        public UserModel(string gmail, string password, bool isActive)
        {
            Gmail = gmail;
            Password = password;
            IsActive = isActive;
        }
    }
}
