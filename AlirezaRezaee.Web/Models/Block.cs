using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public enum BlockPosition: byte
    {
        First_BetweenHomeAndPosts = 0,
        Second_betweenPostsAndLinks = 1,
        Third_BetweenLinksAndFooter = 2
    }

    public class Block
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "برای تنظیم بلاک الزاماً «{0}» باید تعریف شود.")]
        [Display(Name = "محتوا")]
        public string Html { get; set; }

        public string Scripts { get; set; }

        public string Styles { get; set; }

        [Display(Name = "آیا فعال است؟")]
        public bool IsEnable { get; set; }

        public BlockPosition Position { get; set; }

        [Display(Name = "رتبه")]
        public byte? Rank { get; set; }
    }
}
