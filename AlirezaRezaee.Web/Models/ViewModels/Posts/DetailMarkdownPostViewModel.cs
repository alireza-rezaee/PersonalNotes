using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class DetailMarkdownPostViewModel
    {
        public Markdown Markdown { get; set; }

        public string HtmlContent { get; set; }

        public string PostDeleteUrl { get; set; }

        public string PostEditTypeUrl { get; set; }

        public string PostEditProperties { get; set; }

        public string PostDetailUrl { get; set; }
    }
}
