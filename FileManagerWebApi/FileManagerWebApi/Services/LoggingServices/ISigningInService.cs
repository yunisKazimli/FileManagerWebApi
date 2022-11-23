using FileManagerWebApi.Models;
using FileManagerWebApi.Services.JWTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public interface ISigningInService
    {
        public string Authorize(SignInModel userModel, IJWTService jwtService, IUserFileManagerService userFMService);

        public bool Validate(UserModel userModel, SignInModel signModel);
    }
}
