using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Articles
{
    public class CreateEditViewModel
    {
        public Article Article { get; set; }

        public IFormFile Thumbnail { get; set; }

        public IFormFile Cover { get; set; }
    }
}
