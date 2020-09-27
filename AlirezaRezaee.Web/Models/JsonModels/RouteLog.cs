using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.JsonModels
{
    public class RouteLog
    {
        public string Scheme { get; set; }

        public string Host { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }
    }
}
