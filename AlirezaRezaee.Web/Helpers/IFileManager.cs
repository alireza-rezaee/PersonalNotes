using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Helpers
{
    interface IFileManager
    {
        void SaveFile(IFormFile file, string path);

        void DeleteFile(string path);
    }
}
