using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.FileManagerServices
{
    public class UserInfoFileManagerService : IUserInfoFileManagerService
    {
        private readonly string mainDirectoryPath = @"UserFilesFolder";
        private readonly string fileInfoPath = @"UserFilesFolder\InfoFolder\FileInfo.finf";
        private readonly string fileUrlInfoPath = @"UserFilesFolder\InfoFolder\FileUrlInfo.ufinf";

        public void AddFile(string gmail, string fileName, string filePath, string url)
        {
            File.Copy(@$"{filePath}\{fileName}", mainDirectoryPath + @$"\{gmail}\{fileName}");

            File.AppendAllText(fileUrlInfoPath, $"!1!{gmail}!2!{gmail}!3!{fileName}!4!{url}\n");
        }

        public void AddUser(string gmail)
        {
            Directory.CreateDirectory(mainDirectoryPath + @$"\{gmail}");
        }

        public void DeleteFile(string gmail, string fromGmail, string fileName, bool isPersonalFile)
        {
            string fileInfoText = File.ReadAllText(fileInfoPath);
            string fileUrlText = File.ReadAllText(fileUrlInfoPath);

            if (!isPersonalFile)
            {
                fileInfoText = fileInfoText.Remove(fileInfoText.IndexOf($"!1!{fromGmail}!2!{gmail}!3!{fileName}"), ($"!1!{fromGmail}!2!{gmail}!3!{fileName}").Length);

                int urlIndex = fileUrlText.IndexOf($"!1!{fromGmail}!2!{gmail}!3!{fileName}!4!");

                for (int i = urlIndex; i < fileUrlText.Length; i++)
                {
                    if (fileUrlText[i] == '\n')
                    {
                        fileUrlText = fileUrlText.Remove(urlIndex, i + 1 - urlIndex);

                        break;
                    }
                }
            }

            else
            {
                List<string> fileInfoList = fileInfoText.Split('\n').ToList();
                List<string> fileUrlList = fileUrlText.Split('\n').ToList().Where(x => x != "" || x != null).ToList();

                fileInfoText = "";
                fileUrlText = "";

                foreach (string fil in fileInfoList)
                {
                    if (!(fil.IndexOf($"!1!{gmail}!2!") != -1 && fil.IndexOf($"!3!{fileName}") != -1)) fileInfoText += fil + "\n";
                }

                foreach(string ful in fileUrlList)
                {
                    if (!(ful.IndexOf($"!1!{fromGmail}!2!") != -1 && ful.IndexOf($"!3!{fileName}") != -1)) fileUrlText += ful + "\n";
                }

                File.Delete(mainDirectoryPath + $@"\{gmail}\{fileName}");
            }

            File.WriteAllText(fileInfoPath, fileInfoText);
            File.WriteAllText(fileUrlInfoPath, fileUrlText);
        }

        public bool DeleteUser(string gmail)
        {
            throw new NotImplementedException();
        }

        public void DownloadFile(string ownGmail, string fromGmail, string fileName, string destPath)
        {
            File.Copy(mainDirectoryPath + @$"/{fromGmail}/{fileName}", destPath + @$"/{fileName}");
        }

        public void ShareFile(string gmail, string[] toGmail, string[] filesName, string url)
        {
            string textToInfoFile = "";
            string textToUrlInfoFile = "";

            for (int i = 0; i < toGmail.Length; i++)
            {
                for (int j = 0; j < filesName.Length; j++)
                {
                    //check if gmail has file
                    if (!File.Exists(mainDirectoryPath + @$"\{gmail}\{filesName[j]}")) throw new Exception($"there is not exist file : {mainDirectoryPath + @$"\{gmail}\{filesName[j]}"}");

                    //check if toGmail is exist
                    if (!Directory.Exists(mainDirectoryPath + @$"\{toGmail[i]}")) throw new Exception($"there is not exist directory : {mainDirectoryPath + @$"\{toGmail[i]}"}");

                    //check if gmail and toGmail same
                    if (gmail == toGmail[i]) throw new Exception($"source gmail and target gmail cannot be same");

                    //check if gmail already shared this file to toGmail in past
                    if (File.ReadAllText(fileInfoPath).IndexOf($"!1!{gmail}!2!{toGmail[i]}!3!{filesName[j]}") != -1) throw new Exception($"{gmail} already shared {filesName[j]} to {toGmail[i]}");

                    textToInfoFile += $"!1!{gmail}!2!{toGmail[i]}!3!{filesName[j]}\n";

                    textToUrlInfoFile += $"!1!{gmail}!2!{toGmail[i]}!3!{filesName[j]}!4!{url}\n";
                }
            }

            File.AppendAllText(fileInfoPath, textToInfoFile);
            File.AppendAllText(fileUrlInfoPath, textToUrlInfoFile);
        }

        public string[] ShowAllAcceptedFiles(string gmail)
        {
            List<string> allFiles = new List<string>();

            string[] allSharing = File.ReadAllText(fileInfoPath).Split('\n');

            for(int i = 0; i < allSharing.Length; i++)
            {
                if (allSharing[i].IndexOf($"!2!{gmail}!3!") != -1)
                {
                    string fileFullPath;

                    //removed piece of line from FileInfo.finf after !2!
                    fileFullPath = allSharing[i].Remove(allSharing[i].IndexOf("!2!"), allSharing[i].Length - allSharing[i].IndexOf("!2!"));

                    //removed !1! piece(there is only source gmail)
                    fileFullPath = fileFullPath.Remove(0, 3);

                    //added directory path and file name
                    fileFullPath = /*directory name*/mainDirectoryPath + @"\" + /*origin gmail name*/fileFullPath + @"\" + /*file name*/allSharing[i].Remove(0, allSharing[i].IndexOf("!3!") + 3);

                    allFiles.Add(fileFullPath);
                }
            }

            FileInfo[] allOwnFiles = (new DirectoryInfo(mainDirectoryPath + @$"\{gmail}")).GetFiles();

            foreach (FileInfo fi in allOwnFiles) allFiles.Add(fi.FullName);

            return allFiles.ToArray();
        }
        
        public string[] GetAllFilesUrl(string gmail)
        {
            List<string> allFilesUrl = File.ReadAllText(fileUrlInfoPath).Split('\n').ToList();

            allFilesUrl = allFilesUrl.Where(x => x.IndexOf("!2!" + gmail + "!3!") != -1).ToList();

            return allFilesUrl.ToArray();
        }
    }
}
