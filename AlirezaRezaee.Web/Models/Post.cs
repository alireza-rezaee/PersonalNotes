using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Post
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(150, ErrorMessage = "رعایت سقف {1} کاراکتری {0} نوشتار الزامی است.")] //{0}
        [Required(ErrorMessage = "انتخاب {0} برای نوشتار الزامی است.")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [MaxLength(250, ErrorMessage = "رعایت سقف {1} کاراکتری {0} نوشتار الزامی است.")]
        [Required(ErrorMessage = "انتخاب {0} برای نوشتار الزامی است.")]
        public string Summmary { get; set; }

        [Display(Name = "متن")]
        public string HtmlContent { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [Required]
        public DateTime PublishDateTime { get; set; }

        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime LatestUpdateDateTime { get; set; }


        //ToDo: چک شود که آیا روی لیست این شروط پذیرفته است یا با خطا رو به رو می شویم و باید دنبال راه دیگری باشیم
        [Display(Name = "نشانی منبع")]
        [MaxLength(200, ErrorMessage = "رعایت سقف {1} کاراکتری {0} نوشتار الزامی است.")]
        public List<string> SourceUrl { get; set; }
    }
}
