using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts
{
    public class RelatedPostsViewModel
    {
        public PostSummaryViewModel TargetPost { get; set; }

        public List<PostSummaryViewModel> RelatedPosts { get; set; }
    }
}
