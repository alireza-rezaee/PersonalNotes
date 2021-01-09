using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Tags
{
    public class RelatedPostsViewModel
    {
        public ICollection<PostSummaryViewModel> Posts { get; set; }

        public Tag Tag { get; set; }
    }
}
