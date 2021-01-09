using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models.ViewModels
{
    public class OptionViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string AvatarOrginalPath { get; set; }

        public string AvatarPath_64px { get; set; }

        public string AvatarPath_100px { get; set; }

        public string AvatarPath_125px { get; set; }

        public string AvatarPath_150px { get; set; }

        public string CoverPath { get; set; }

        public string IllustratedNamePath { get; set; }

        public string SiteFootnote { get; set; }

        public string QuranAyah { get; set; }

        public string AboutAuthorSummary { get; set; }
    }
}
