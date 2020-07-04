using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Blocks
{
    public class CreateEditViewModel
    {
        [Required(ErrorMessage = "برای تنظیم بلاک الزاماً «{0}» باید تعریف شود.")]
        [Display(Name = "محتوا")]
        public string Html { get; set; }

        public bool IsEnable { get; set; }

        public byte? PositionNo { get; set; }

        public byte? Rank { get; set; }
    }
}
