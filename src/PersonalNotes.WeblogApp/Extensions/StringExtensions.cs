using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Extensions
{
    public static class StringExtensions
    {
        public static string EnglishNumberToPersian(this string EnglishStr)
        {
            if (EnglishStr == null) throw new ArgumentNullException(nameof(EnglishStr));

            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['0'] = '۰',
                ['1'] = '۱',
                ['2'] = '۲',
                ['3'] = '۳',
                ['4'] = '۴',
                ['5'] = '۵',
                ['6'] = '۶',
                ['7'] = '۷',
                ['8'] = '۸',
                ['9'] = '۹'
            };
            foreach (var item in EnglishStr)
            {
                if (LettersDictionary.ContainsKey(item))
                    EnglishStr = EnglishStr.Replace(item, LettersDictionary[item]);
            }
            return EnglishStr;
        }

        public static string PersianNumberToEnglish(this string persianStr)
        {
            if (persianStr == null) throw new ArgumentNullException(nameof(persianStr));

            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                if (LettersDictionary.ContainsKey(item))
                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            return persianStr;
        }

        public static string ControllerName(this string className)
        {
            if (className is null)
                throw new ArgumentNullException(nameof(className));
            if (!className.EndsWith(nameof(Controller)) || className.Length <= nameof(Controller).Length)
                throw new ArgumentException("`className` does not seem to be `Controller` name!", nameof(className));
            if (!SyntaxFacts.IsValidIdentifier(className))
                throw new ArgumentException("`className` has invalid characters.", nameof(className));
            return className[0..^10];
        }
    }
}
