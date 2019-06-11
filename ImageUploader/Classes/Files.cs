using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Uploader
{
    public static  class Files
    {
        /// <summary>
        /// Uploud Image File with validation
        /// </summary>
        /// <param name="url">the location to save the image</param>
        /// <param name="file">the file of type IFormFile</param>
        /// <returns>the name of saved image</returns>
        public static async Task<string> Upload(string path, IFormFile file,bool zipit=false)
        {
   

            return await WriteFile(path, file, zipit);

        }
         

        /// <summary>
        /// Delete the image from the local folder
        /// </summary>
        /// <param name="url">the path of image</param>
        /// <param name="filename">the file name</param>
        /// <returns>the deletion status</returns>
        public static bool Delete(string path, string filename)
        {
            if (DeleteFile(path, filename))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
 

        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static async Task<string> WriteFile(string url, IFormFile file,bool zipit)
        {
            string fileName;
            try
            {
                if (string.IsNullOrEmpty(url))
                    url = "wwwroot";
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var zipextension = ".zip";
               fileName = Guid.NewGuid().ToString(); //Create a new Name for the file due to security reasons.
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), url, fileName + extension);

                using (var bits = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                if (zipit)
                {
                    string zipPath = Path.Combine(Directory.GetCurrentDirectory(), url, fileName+ zipextension);
                    // string extractPath = @".\extract";

                    //  ZipFile.CreateFromDirectory(path, zipPath);

                    // ZipFile.ExtractToDirectory(zipPath, extractPath);

                 
                   // string extractPath = @"c:\users\exampleuser\extract";
                    //string newFile = @"c:\users\exampleuser\NewFile.txt";

                    using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                    {
                        archive.CreateEntryFromFile(filepath, fileName + extension);
                      //  archive.ExtractToDirectory(extractPath);
                    }



                    DeleteFile(Path.Combine(Directory.GetCurrentDirectory(), url), fileName + extension);
                }



            }
            catch (Exception e)
            {

                return null;
            }

            return fileName;
        }


        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool DeleteFile(string url, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    url = "wwwroot";

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), url, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        
    }
}
