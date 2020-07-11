﻿using Rezaee.Alireza.Web.Helpers.Enums;
using Rezaee.Alireza.Web.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Tags
{
    public class PostSummaryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime PublishDateTime { get; set; }

        public DateTime? LatestUpdateDateTime { get; set; }

        public PostType? Type { get; set; }

        public string PostUrl { get; set; }
    }
}