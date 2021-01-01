using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Extensions
{
    public static class StringExtensions
    {
        public static string ControllerName(this string className) => className[0..^10];
    }
}
