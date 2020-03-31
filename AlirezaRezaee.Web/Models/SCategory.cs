using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class SCategory
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "انتخاب {0} برای دسته بندی الزامی است.")]
        public string Title { get; set; }

        public ICollection<ShareCategory> ShareCategories { get; set; }
    }
}
