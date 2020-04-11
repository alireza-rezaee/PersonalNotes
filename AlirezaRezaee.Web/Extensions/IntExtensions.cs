using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Extensions
{
    public static class IntExtensions
    {
        public static string EnglishToPersian(this string EnglishStr)
        {
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

        public static string PersianToEnglish(this string persianStr)
        {
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

        public static string PersianToEnglish(this int persianNumber) => PersianToEnglish(persianNumber.ToString());

        public static string EnglishToPersian(this int EnglishStr) => EnglishToPersian(EnglishStr.ToString());
    }
}
