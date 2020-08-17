using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Extensions
{
    public static class IHtmlHelperExtensions
    {
        public static string YesOrNo(this IHtmlHelper htmlHelper, bool status) => status ? "بلی" : "خیر";
    }
}
