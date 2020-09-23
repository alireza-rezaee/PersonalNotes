using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rezaee.Alireza.Web.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.RouteConstraint
{
    public class PageConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object value))
            {
                var parameterValueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (parameterValueString == null)
                {
                    return false;
                }

                try
                {
                    var context = (ApplicationDbContext)httpContext.RequestServices.GetService(typeof(ApplicationDbContext));
                    return context.Pages.Any(page => page.Path == parameterValueString);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return false;
        }
    }
}
