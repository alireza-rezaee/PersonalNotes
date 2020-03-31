using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class ShareCategory
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        [ForeignKey("CategoryId")]
        public ACategory Category { get; set; }
    }
}
