using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class PostSummaryViewModel : Post
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string PostUrl { get; set; }
    }
}
