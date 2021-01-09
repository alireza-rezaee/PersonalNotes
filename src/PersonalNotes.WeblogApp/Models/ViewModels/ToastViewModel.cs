using AlirezaRezaee.PersonalNotes.WeblogApp.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels
{
    public class ToastViewModel
    {
        public ToastType? Type { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime? Time { get; set; }

        public bool? Animation { get; set; }

        public bool? Autohide { get; set; }

        public int? Delay { get; set; }
    }
}
