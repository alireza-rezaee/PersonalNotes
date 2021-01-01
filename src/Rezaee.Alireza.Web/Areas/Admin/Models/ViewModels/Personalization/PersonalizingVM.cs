using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Areas.Admin.Models.ViewModels.Personalization
{
    public class PersonalizingVM
    {
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display( Name = "عنوان صفحه نخست")]
        public string IndexTitle { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "توضیح صفحه نخست")]
        public string IndexDescription { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "عنوان افزوده")]
        public string AdditionalTitle { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "پانوشت")]
        public string SiteFootnote { get; set; }

        public IFormFile SiteCover { get; set; }

        public IFormFile SiteLogo { get; set; }

        public IFormFile TextLogo { get; set; }
    }
}
