using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Areas.Admin.Models
{
    public class File
    {
        public string Name
        {
            get => Path.GetFileName(FilePath);
        }

        [Key]
        [Required(ErrorMessage = "انتخاب {0} فایل الزامی است.")]
        [MaxLength(250)]
        [Display(Name = "نام")]
        public string FilePath { get; set; }

        [Required(ErrorMessage = "انتخاب {0} فایل الزامی است.")]
        [MaxLength(32, ErrorMessage = "سقف طول {0} {1} کاراکتر است.")]
        [Display(Name = "پسوند")]
        public string ContentType { get; set; }

        [Display(Name = "حجم")]
        public long Length { get; set; }

        [Display(Name = "زمان ایجاد")]
        public DateTime CreateDateTime { get; set; }
    }
}
