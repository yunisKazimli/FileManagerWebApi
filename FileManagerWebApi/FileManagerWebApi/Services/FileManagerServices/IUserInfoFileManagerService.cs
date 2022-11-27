using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.FileManagerServices
{
    public interface IUserInfoFileManagerService
    {
        public void AddUser(string gmail);

        public void DownloadFile(string ownGmail, string fromGmail, string fileName, string destPath);

        public bool DeleteUser(string gmail);

        public void AddFile(string gmail, string fileName, string filePath, string url);

        public void ShareFile(string gmail, string[] toGmail, string[] filesName, string url);

        public void DeleteFile(string gmail, string fromGmail, string fileName, bool isPersonalFile);

        public string[] ShowAllAcceptedFiles(string gmail);

        public string[] GetAllFilesUrl(string gmail);
    }
}
