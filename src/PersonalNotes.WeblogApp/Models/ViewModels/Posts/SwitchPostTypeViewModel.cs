using AlirezaRezaee.PersonalNotes.WeblogApp.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels.Posts
{
    public class SwitchPostTypeViewModel
    {
        public PostType PostType { get; set; }

        public string PostTypeName { get; set; }

        public string SwitchUrl { get; set; }
    }
}
