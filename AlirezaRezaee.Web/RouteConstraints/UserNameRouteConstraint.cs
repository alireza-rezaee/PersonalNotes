using AlirezaRezaee.Web.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.RouteConstraints
{
    public class UsernameRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // check nulls
            if (values.TryGetValue(routeKey, out object value) && value != null)
            {
                return (httpContext.RequestServices.GetService(typeof(IUserService)) as IUserService)
                    .IsExists(Convert.ToString(value));
            }

            return false;
        }
    }
}
