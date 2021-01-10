using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Extensions
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
            Check(file, allowedTypes);
            if (file.Length > maxSize) throw new ArgumentOutOfRangeException("فایل انتخاب شده بزرگتر از حد مجاز می باشد.");
        }

        /// <summary>
        /// Check uploaded file 
        /// </summary>
        /// <param name="file">uploaded file </param>
        /// <param name="allowedTypes">Allowed MIME types to upload </param>
        public static void Check(this IFormFile file, string[] allowedTypes)
        {
            if (file is null) throw new ArgumentNullException(nameof(file), "فایلی انتخاب نشده است.");
            if (allowedTypes is null) throw new ArgumentNullException(nameof(allowedTypes), "نوع مجازی برای راستی آزمایی فایل نباید خالی باشد.");
            if (file.Length == 0) throw new ArgumentOutOfRangeException("اندازه فایل بسیار کوچک است.", nameof(file));
            if (!allowedTypes.Any()) throw new ArgumentException("حداقل یک نوع برای راستی آزمایی فایل باید انتخاب شود.", nameof(file));
            if (!allowedTypes.Contains(file.ContentType)) throw new Exception("فایل انتخاب شده در قالب مجاز نمی باشد.");
        }
    }
}
