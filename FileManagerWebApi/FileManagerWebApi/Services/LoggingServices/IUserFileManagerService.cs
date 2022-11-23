using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public interface IUserFileManagerService
    {
        public string WriteNewUser(UserModel newUserModel);

        public string ReadAllUsers();

        public UserModel ReadUser(string gmail);
    }
}
