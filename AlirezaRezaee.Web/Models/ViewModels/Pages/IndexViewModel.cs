using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Pages
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "عنوان برگه")]
        public string Title { get; set; }

        [Display(Name = "زمان به‌روز رسانی")]
        public DateTime? UpdateDateTime { get; set; }

        [Display(Name = "نشانی")]
        public string Path { get; set; }
    }
}
