using AlirezaRezaee.PersonalNotes.WeblogApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Areas.Identity.Helpers
{
    public enum Language
    {
        English,
        Persian
    }

    public static class Describer
    {
        public static string UnableToLoadUser(string userid = "", Language lang = Language.English)
        {
            if (string.IsNullOrEmpty(userid))
                if (lang == Language.English)
                    return "Unable to load user.";
                else
                    return "کاربری یافت نشد.";

            if (lang == Language.English)
                return $"Unable to load user with ID '{userid}'.";
            else
                return $"کاربری با شناسه '{userid}' یافت نشد.";
        }
    }
}
