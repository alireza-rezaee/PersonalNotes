﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Posts
{
    public class CreateEditSharePostViewModel
    {
        public Post Post { get; set; }

        public Share Share { get; set; }

        public IFormFile ThumbnailImage { get; set; }
    }
}
