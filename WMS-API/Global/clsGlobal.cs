using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WMS_CQRS_Business_Layer.Global
{
    public static class clsGlobal
    {
        public static async Task<string?> SavePictureAsync(IFormFile file, string Foldername)
        {
            try
            {
                if (file == null || file.Length == 0 || Foldername.Length == 0)
                    throw new ArgumentException("Invalid file");

                string folder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName, Foldername);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(folder, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Path.Combine(Foldername, uniqueFileName).Replace("\\", "/");

            }
            catch(Exception)
            {
                return null;
            }


        }

        public static bool DeleteFile(string relativePath)
        {
            try
            {
                // Normalize slashes and split folder and file
                relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                // Get root solution directory
                string rootPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName;

                // Combine with relative path to get full file path
                string fullPath = Path.Combine(rootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false; // File not found
            }
            catch (Exception)
            {
                // Optionally log the exception
                return false;
            }
        }

        public static FileStreamResult? GetImageAsStreamResult(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                    return null;

                // Prevent path traversal and absolute paths
                if (relativePath.Contains("..") || Path.IsPathRooted(relativePath))
                    return null;

                relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                // Navigate up 2 directories safely
                var rootDir = Directory.GetParent(Directory.GetCurrentDirectory());
                if (rootDir?.Parent == null)
                    return null;

                string root = rootDir.Parent.FullName;
                string fullPath = Path.Combine(root, relativePath);

                if (!File.Exists(fullPath))
                    return null;

                string contentType = GetContentType(fullPath); // Define this method appropriately

                var stream = new FileStream(
                    fullPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    useAsync: true
                );

                return new FileStreamResult(stream, contentType);
            }
            catch
            {
                // You might log the error here if needed
                return null;
            }

        }

        private static string GetContentType(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

    }
}
