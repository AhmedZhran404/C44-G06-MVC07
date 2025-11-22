using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        public AttachmentService(IWebHostEnvironment webHostEnvironment)
        {
            this._webHost = webHostEnvironment;
        }

        private readonly string[] allowedExtensions = { ".jpg", "jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly IWebHostEnvironment _webHost;


        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file == null || file.Length == 0) return null;

                // Check MaxSize
                if (file.Length > maxFileSize) return null;

                var extention = Path.GetExtension(file.FileName).ToLower();

                // Check Exsetention
                if (!allowedExtensions.Contains(extention)) 
                    return null;

                // wwwroot/images/members
                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + extention;
                // wwwroot/images/members/1s337g7ehf84hf84n.pnj
                var filePath = Path.Combine(folderPath, fileName);

                using var filestream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(filestream);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falid To Upload File To Folder {folderName} : {ex}");
                return null;
            }
        
        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var FullPath = Path.Combine(_webHost.WebRootPath, "images" , folderName , fileName);

                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }

                return false;

            }catch(Exception ex) 
            {
                Console.WriteLine($"Faild To Delete File Name {fileName} : {ex}");
                return false;
            }
        }

    }

}
