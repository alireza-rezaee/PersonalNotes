using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Helpers
{
    public class File
    {
        public static string ValidateName(string Name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                Name = Name.Replace(c, ' ');
            return Regex.Replace(Name, @"\s+", "-");
        }
    }
}
