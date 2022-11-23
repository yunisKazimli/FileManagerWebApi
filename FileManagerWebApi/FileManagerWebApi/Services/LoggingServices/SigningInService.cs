using FileManagerWebApi.Models;
using FileManagerWebApi.Services.JWTService;
using System;

namespace FileManagerWebApi.Services.LoggingServices
{
    public class SigningInService : ISigningInService
    {
        public string Authorize(SignInModel signInModel, IJWTService jwtService, IUserFileManagerService userFMService)
        {
            UserModel userModel;

            userModel = userFMService.ReadUser(signInModel.Gmail);

            if (!Validate(userModel, signInModel)) throw new Exception("Invalid Gmail or Password");

            if (!userModel.IsActive) throw new Exception("Account still require activation");

            return jwtService.GetToken(userModel);
        }

        public bool Validate(UserModel userModel, SignInModel signModel)
        {
            return userModel.Gmail != null && StaticClasses.HashManager.CompareHash(userModel.Password, signModel.Password);
        }
    }
}
