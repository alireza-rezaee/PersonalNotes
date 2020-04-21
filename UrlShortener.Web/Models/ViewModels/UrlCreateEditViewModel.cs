using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Web.Models.ViewModels
{
    public class UrlCreateEditViewModel
    {
        [Required(ErrorMessage = "{0} را فراموش کردید؟ وارد کردن {0} الزامی است.")]
        [Display(Name = "نشانی")]
        public string Url { get; set; }

        [Display(Name = "نشانی سفارشی؟")]
        public bool IsCustomShortLink { get; set; }

        [Display(Name = "نشانی دلخواه")]
        public string CustomShortLink { get; set; }

        [Required(ErrorMessage = "لطفاً فرایند احراز هویت \"{0}\" را انجام دهید.")]
        [Display(Name = "من ربات نیستم")]
        public string Recaptcha { get; set; }
    }
}
