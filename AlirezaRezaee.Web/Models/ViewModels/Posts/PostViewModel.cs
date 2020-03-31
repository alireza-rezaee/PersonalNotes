using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class PostViewModel
    {
        public IEnumerable<Article> Articles { get; set; }

        public IEnumerable<Share> Shares { get; set; }
    }
}
