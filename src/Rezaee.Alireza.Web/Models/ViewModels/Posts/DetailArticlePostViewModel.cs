using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Posts
{
    public class DetailArticlePostViewModel
    {
        public Post Post { get; set; }

        public string PostDeleteUrl { get; set; }

        public string PostEditTypeUrl { get; set; }

        public string PostEditUrl { get; set; }

        public string PostDetailUrl { get; set; }
    }
}
