using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "سقف طول مجاز {0} {1} کاراکتر است.")]
        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [Display(Name = "عنوان پیام")]
        public string Title { get; set; }

        [MaxLength(100, ErrorMessage = "سقف طول مجاز {0} {1} کاراکتر است.")]
        [Display(Name = "نام نویسنده")]
        public string SenderName { get; set; }

        [MaxLength(150, ErrorMessage = "سقف طول مجاز {0} {1} کاراکتر است.")]
        [EmailAddress(ErrorMessage = "لطفاً {0} را به صورت صحیح وارد کنید.")]
        [Display(Name = "نشانی رایانامه")]
        public string SenderEmail { get; set; }

        [MaxLength(1000, ErrorMessage = "سقف طول مجاز {0} {1} کاراکتر است.")]
        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "پیام")]
        public string Content { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public bool HaveRead { get; set; }
    }
}
