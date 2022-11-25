using FileManagerWebApi.Models;
using FileManagerWebApi.Services.FileManagerServices;
using FileManagerWebApi.Services.JWTService;
using FileManagerWebApi.Services.LoggingServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileManagerWebApi.Controllers
{
    public class UserLoggingController : Controller
    {
        private readonly ISigningUpService signingUpService;
        private readonly ISigningInService signingInService;
        private readonly IUserFileManagerService userFMService;
        private readonly IJWTService jwtService;
        private readonly IUserInfoFileManagerService userInfoFMService;

        public UserLoggingController(ISigningUpService _signingUpService, ISigningInService _signingInService, IUserFileManagerService _userFMService, IJWTService _jwtService, IUserInfoFileManagerService _userInfoFMService)
        {
            signingUpService = _signingUpService;
            userFMService = _userFMService;
            jwtService = _jwtService;
            signingInService = _signingInService;
            userInfoFMService = _userInfoFMService;
        }

        [HttpGet("SignIn")]
        public IActionResult SignIn(/*SignInModel signInModel*/string Gmail, string Password)
        {
            SignInModel signInModel = new SignInModel(Gmail, Password);

            string token;

            try
            {
                token = signingInService.Authorize(signInModel, jwtService, userFMService);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            return Ok(token);
        }

        [/*HttpPost*/HttpGet("SignUp")]
        public IActionResult SignUp(/*SignUpModel signUpModel*/string Gmail, string Password/*, string ConfirmPassword*/)
        {
            SignUpModel signUpModel = new SignUpModel(Gmail, Password/*, ConfirmPassword*/);

            try
            {
                signingUpService.SignUp(signUpModel, userFMService);

                userInfoFMService.AddUser(signUpModel.Gmail);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("Success");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
