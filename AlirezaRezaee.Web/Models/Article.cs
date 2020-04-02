using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Article : Post
    {
        public int ArticleId { get; set; }

        [Display(Name = "متن")]
        public string HtmlContent { get; set; }

        [Display(Name = "نشانی منبع")]
        public string SourcesUrl { get; set; }

        public string CoverUrl { get; set; }

        public ICollection<ArticleCategory> ArticleCategories { get; set; }

    }
}
