using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.JsonModels
{
    public class RequestResponseLog
    {
        public SummaryLog Summary { get; set; }

        public List<StringCouple> RequestHeaders { get; set; }

        public List<StringCouple> ResponseHeaders { get; set; }
    }
}
