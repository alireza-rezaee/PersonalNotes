using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        //[Required] -> It was removed due to speeding up the registration
        [StringLength(100)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [PersonalData]
        //[Required] -> It was removed due to speeding up the registration
        [StringLength(100)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [PersonalData]
        [StringLength(100)]
        [Display(Name = "نام نمایشی")]
        public string DisplayName { get; set; }

        [PersonalData]
        [Display(Name = "زادروز")]
        public DateTime? BirthDate { get; set; }

        [PersonalData]
        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisteredDateTime { get; set; }

        [StringLength(150)]
        [Display(Name = "نشانی تصویر")]
        public string ProfileImagePath { get; set; }

        [StringLength(100)]
        [Display(Name = "محل زندگی")]
        public string Location { get; set; }
    }
}
