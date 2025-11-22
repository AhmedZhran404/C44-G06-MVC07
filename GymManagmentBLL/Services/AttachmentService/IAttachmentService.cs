using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        // "IFormFile represents a file sent by the client in an HTTP request,
        // typically uploaded through a form."
        string? Upload(string folderName, IFormFile file);

        bool Delete(string fileName, string folderName);

      
    }
}
