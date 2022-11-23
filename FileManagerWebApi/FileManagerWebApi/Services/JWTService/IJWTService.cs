using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.JWTService
{
    public interface IJWTService
    {
        public string GetToken(UserModel userModel);
    }
}
