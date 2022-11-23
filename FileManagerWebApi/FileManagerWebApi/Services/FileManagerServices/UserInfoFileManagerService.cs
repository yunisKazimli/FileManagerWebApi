using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerWebApi.Services.FileManagerServices
{
    public class UserInfoFileManagerService : IUserInfoFileManagerService
    {
        private readonly string mainDirectoryPath = @"D:\Projects\VSprojects\FileManagerWebApi\UserFilesFolder";
        private readonly string fileInfoPath = @"D:\Projects\VSprojects\FileManagerWebApi\UserFilesFolder\InfoFolder\FileInfo.finf";

        public void AddFile(string gmail, string fileName, string filePath)
        {
            File.Copy(@$"{filePath}\{fileName} ", mainDirectoryPath + @$"\{gmail}\{fileName}");
        }

        public void AddUser(string gmail)
        {
            Directory.CreateDirectory(mainDirectoryPath + @$"\{gmail}");
        }

        public void DeleteFile(string gmail, string fileName, bool isPersonalFile)
        {
            string fileInfoText = File.ReadAllText(fileInfoPath);

            if (!isPersonalFile)
            {
                int fileIndex = fileInfoText.IndexOf($"!2!{gmail}!3!{fileName}");

                for (int i = fileIndex - 1; i >= 0; i--)
                {
                    if (fileInfoText[i] == '!' && fileInfoText[i - 1] == '1' && fileInfoText[i - 2] == '!')
                    {
                        fileInfoText = fileInfoText.Remove(i - 2, fileIndex + ($"!2!{gmail}!3!{fileName}").Length + 1);

                        break;
                    }
                }
            }

            else
            {
                List<string> fileInfoList = fileInfoText.Split('\n').ToList();

                fileInfoText = "";

                foreach (string fil in fileInfoList)
                {
                    if (!(fil.IndexOf($"!1!{gmail}!2!") != -1 && fil.IndexOf($"!3!{fileName}") != -1)) fileInfoText += fil + "\n";
                }

                File.Delete(mainDirectoryPath + $@"\{gmail}\{fileName}");
            }

            File.WriteAllText(fileInfoPath, fileInfoText);
        }

        public bool DeleteUser(string gmail)
        {
            throw new NotImplementedException();
        }

        public void ShareFile(string gmail, string[] toGmail, string[] filesName)
        {
            string text = "";

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

                    text += $"!1!{gmail}!2!{toGmail[i]}!3!{filesName[j]}\n";
                }
            }

            File.AppendAllText(fileInfoPath, text);
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
    }
}
