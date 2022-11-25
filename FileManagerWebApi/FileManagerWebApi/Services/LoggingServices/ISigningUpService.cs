using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public interface ISigningUpService
    {
        public void SignUp(SignUpModel signUpModel, IUserFileManagerService userFM);

        public string[] GetAllGmails();
    }
}
