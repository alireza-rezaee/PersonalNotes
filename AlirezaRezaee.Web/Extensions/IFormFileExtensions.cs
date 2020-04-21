using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Extensions
{
    public static class IFormFileExtensions
    {
        /// <summary>
        /// Check uploaded file 
        /// </summary>
        /// <param name="file">uploaded file </param>
        /// <param name="maxSize">Maximum allowed size to upload</param>
        /// <param name="allowedTypes">Allowed MIME types to upload </param>
        public static void Check(this IFormFile file, long maxSize, string[] allowedTypes)
        {
            if (file == null || file.Length == 0)
                throw new Exception("فایلی انتخاب نشده است.");

            if (file.Length > maxSize)
                throw new Exception("فایل انتخاب شده بزرگتر از حد مجاز می باشد.");

            if (!allowedTypes.Contains(file.ContentType))
                throw new Exception("فایل انتخاب شده برای محصول در قالب مجاز نمی باشد.");
        }
    }
}
