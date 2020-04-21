using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Markdown
    {
        [Display(Name = "عنوان نشانی")]
        [MaxLength(150, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        public string UrlTitle { get; set; }

        [Display(Name = "مسیر فایل")]
        [MaxLength(400, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        public string FileUrl { get; set; }

        [Display(Name = "نشانی ویرایش فایل")]
        [MaxLength(400, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        public string ContributeUrl { get; set; }

        #region Relations
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
        #endregion
    }
}
