using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(150, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        [Required(ErrorMessage = "انتخاب {0} برای نوشتار الزامی است.")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [MaxLength(250, ErrorMessage = "رعایت سقف {1} کاراکتری {0} ارسالی الزامی است.")]
        [Required(ErrorMessage = "انتخاب {0} برای ارسالی الزامی است.")]
        public string Summary { get; set; }

        public string ThumbnailUrl { get; set; }

        [Display(Name = "تاریخ انتشار")]
        public DateTime PublishDateTime { get; set; }

        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime? LatestUpdateDateTime { get; set; }

        #region Relations
        public Article Article { get; set; }

        public Share Share { get; set; }

        public Markdown Markdown { get; set; }

        public Recommendeds RecommendedPost { get; set; }

        public DestructivePost DestructivePosts { get; set; }

        public ICollection<PostTag> PostTags { get; set; }

        public Pin Pin { get; set; }
        #endregion
    }
}
