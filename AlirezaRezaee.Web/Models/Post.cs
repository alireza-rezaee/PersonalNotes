using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Post
    {
        [Display(Name = "عنوان")]
        [MaxLength(150, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        [Required(ErrorMessage = "انتخاب {0} برای نوشتار الزامی است.")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [MaxLength(250, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        [Required(ErrorMessage = "انتخاب {0} برای ارسالی الزامی است.")]
        public string Summmary { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [Required]
        public DateTime PublishDateTime { get; set; }

        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime LatestUpdateDateTime { get; set; }
    }
}
