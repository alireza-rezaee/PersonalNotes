using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.JsonModels
{
    public class SummaryLog
    {
        public string RequestMethod { get; set; }

        public RouteLog RequestRoute { get; set; }

        public int RequestStatusCode { get; set; }

        public string RequestProtocol { get; set; }

        public long? ResponseContentLength { get; set; }

        public string RequestReferrer { get; set; }

        public string RequestIpAddress { get; set; }

        public string RequestId { get; set; }

        public string Time { get; set; }
    }
}
