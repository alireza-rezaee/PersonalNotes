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
        public string FirstName { get; set; }

        [PersonalData]
        //[Required] -> It was removed due to speeding up the registration
        [StringLength(100)]
        public string LastName { get; set; }

        [PersonalData]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [PersonalData]
        public DateTime? BirthDate { get; set; }

        [PersonalData]
        public DateTime RegisteredDateTime { get; set; }

        [StringLength(150)]
        public string ProfileImagePath { get; set; }

        [StringLength(100)]
        [Display(Name = "محل زندگی")]
        public string Location { get; set; }
    }
}
