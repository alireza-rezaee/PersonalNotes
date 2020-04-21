using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Helpers
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _env;
        public FileManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void DeleteFile(string path)
        {
            var absolutePath = _env.WebRootPath + path;
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);
        }

        public async Task SaveFile(IFormFile file, string path)
        {
            var absolutePath = Path.GetFullPath(path, _env.WebRootPath);

            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

            using (var fileStream = new FileStream(absolutePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
