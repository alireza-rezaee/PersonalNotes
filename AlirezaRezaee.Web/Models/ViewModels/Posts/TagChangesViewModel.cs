using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Posts
{
    public class TagChangesViewModel
    {
        public List<string> NewTags { get; set; }

        public List<PostTag> RemoveTags { get; set; }
    }
}
