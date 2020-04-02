using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Articles
{
    public class ArticleSummary : Post
    {
        [Key]
        public int ArticleId { get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
