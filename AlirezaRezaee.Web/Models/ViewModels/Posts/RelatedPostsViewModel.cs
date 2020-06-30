using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Posts
{
    public class RelatedPostsViewModel
    {
        public PostSummaryViewModel TargetPost { get; set; }

        public List<PostSummaryViewModel> RelatedPosts { get; set; }
    }
}
