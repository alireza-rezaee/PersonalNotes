using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models
{
    public class Page
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = "سقف مجاز طول «{0}» «{1}» کاراکتر، باید رعایت شود.")]
        [Required(ErrorMessage = "«{0}» صفحه انتخاب نشده است؟")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "«{0}» صفحه انتخاب نشده است؟")]
        [Display(Name = "محتوا")]
        public string Html { get; set; }

        [Display(Name = "آیا محتوا شامل پوسته مستقل است؟")]
        public bool HasLayout { get; set; }

        [Display(Name = "زمان ایجاد")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "زمان آخرین به روز رسانی")]
        public DateTime? UpdateDateTime { get; set; }

        [Display(Name = "آیا قابل رویت است؟")]
        public bool IsVisible { get; set; }

        [Display(Name = "تصویر جلد")]
        public string ImageCoverPath { get; set; }

        [MaxLength(200, ErrorMessage = "سقف مجاز طول «{0}» «{1}» کاراکتر، باید رعایت شود.")]
        [Required(ErrorMessage = "«{0}» صفحه انتخاب نشده است؟")]
        [Display(Name = "نشانی")]
        public string Path { get; set; }
    }
}
