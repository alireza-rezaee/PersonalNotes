using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Extensions
{
    public static class IntExtensions
    {
        public static string EnglishToPersian(this int EnglishStr) => StringExtensions.EnglishNumberToPersian(EnglishStr.ToString());

        public static Guid ToGuid(this int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
