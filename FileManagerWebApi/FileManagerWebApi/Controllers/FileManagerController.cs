using FileManagerWebApi.Services.FileManagerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly IUserInfoFileManagerService userInfoFLservice;

        public FileManagerController(IUserInfoFileManagerService _userInfoFLservice)
        {
            userInfoFLservice = _userInfoFLservice;
        }

        [Authorize("Bearer")]
        [HttpPost("AddFile")]
        public IActionResult AddFile(string fileName, string filePath)
        {
            string gmail = "";

            try
            {
                userInfoFLservice.AddFile(gmail, fileName, filePath);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("success");
        }

        [Authorize("Bearer")]
        [HttpPost("ShareFile")]
        public IActionResult ShareFile(string[] toGmails, string[] filesName)
        {
            string gmail = "";

            try
            {
                userInfoFLservice.ShareFile(gmail, toGmails, filesName);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("success");
        }

        [Authorize("Bearer")]
        [HttpGet("AllAcceptedFiles")]
        public IActionResult ShowAllAcceptedFiles()
        {
            string gmail = "";

            string[] allSharedFiles;

            try
            {
                allSharedFiles = userInfoFLservice.ShowAllAcceptedFiles(gmail);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok(allSharedFiles);
        }

        [Authorize("Bearer")]
        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(string fileName, bool isPersonal)
        {
            string gmail = "";

            try
            {
                userInfoFLservice.DeleteFile(gmail, fileName, isPersonal);
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
