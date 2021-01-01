using Rezaee.Alireza.Web.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.ViewModels.Posts
{
    public class SwitchPostTypeViewModel
    {
        public PostType PostType { get; set; }

        public string PostTypeName { get; set; }

        public string SwitchUrl { get; set; }
    }
}
