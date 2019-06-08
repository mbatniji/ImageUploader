using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploader
{
    public static class ImageUploader
    {
        /// <summary>
        /// Uploud Image File with validation
        /// </summary>
        /// <param name="url">the location to save the image</param>
        /// <param name="file">the file of type IFormFile</param>
        /// <returns>the name of saved image</returns>
        public static async Task<string> UploadImage(string url, IFormFile file)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFile(url, file);
            }

            return null;
        }

        /// <summary>
        /// Delete the image from the local folder
        /// </summary>
        /// <param name="url">the path of image</param>
        /// <param name="filename">the file name</param>
        /// <returns>the deletion status</returns>
        public static bool DeleteImage(string url, string filename)
        {
            if (DeleteFile(url, filename))
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

        private static bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return GetImageFormat(fileBytes) != ImageFormat.unknown;
        }

        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static async Task<string> WriteFile(string url, IFormFile file)
        {
            string fileName;
            try
            {
                if (string.IsNullOrEmpty(url))
                    url = "wwwroot";
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name for the file due to security reasons.
                var path = Path.Combine(Directory.GetCurrentDirectory(), url, fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
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


        private enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }


        private static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }
    }
}
