using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts
{
    public class CreateEditMarkdownPostViewModel
    {
        public Post Post { get; set; }

        public Markdown Markdown { get; set; }

        public IFormFile ThumbnailImage { get; set; }

        public string PostTags { get; set; }
    }
}
