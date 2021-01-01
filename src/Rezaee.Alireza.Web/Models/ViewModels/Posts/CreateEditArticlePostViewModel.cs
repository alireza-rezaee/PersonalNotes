using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Posts
{
    public class CreateEditArticlePostViewModel
    {
        public Post Post { get; set; }

        public Article Article { get; set; }

        public IFormFile ThumbnailImage { get; set; }

        public IFormFile CoverImage { get; set; }

        public string PostTags { get; set; }
    }
}
