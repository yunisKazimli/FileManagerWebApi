using FileManagerWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.LoggingServices
{
    public class UserFileManagerService : IUserFileManagerService
    {
        private readonly string userInfoPath = @"UserFilesFolder\InfoFolder\UserInfo.uinf";

        public string ReadAllUsers()
        {
            string allUsersString;

            allUsersString = File.ReadAllText(userInfoPath);

            return allUsersString;
        }

        public UserModel ReadUser(string gmail)
        {
            string allUsersString;

            allUsersString = File.ReadAllText(userInfoPath);

            if (allUsersString.IndexOf("!" + gmail + "!") == -1) throw new Exception("this gmail didn't sign up");

            return FromStringToUser(allUsersString, gmail);
        }

        public string WriteNewUser(UserModel newUserModel)
        {
            string newLine = "!" + newUserModel.Gmail + "!" + newUserModel.Password + "!" + newUserModel.IsActive + "\n";

            File.AppendAllText(userInfoPath, newLine);

            return newLine;
        }

        //read file content and create userModel based on that
        private UserModel FromStringToUser(string allUsersString, string gmail)
        {
            string password = "";
            bool isActive = false;

            for (int i = allUsersString.IndexOf(gmail + "!") + gmail.Length + 1; i < allUsersString.Length; i++)
            {
                if (allUsersString[i] != '!') password += allUsersString[i];
                else
                {
                    isActive = allUsersString[i + 1] == 'T';

                    break;
                }
            }

            return new UserModel(gmail, password, isActive);
        }
    }
}
