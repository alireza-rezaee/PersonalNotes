using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class RequestResponseDetails
    {
        [MaxLength(10)]
        public string Protocol { get; set; }

        [MaxLength(2048)]
        public string Host { get; set; }

        [MaxLength(2048)]
        public string Referrer { get; set; }

        public string RequestHeaders { get; set; }

        public string ResponseHeaders { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string Exception { get; set; }

        #region Relations
        [Key]
        [ForeignKey("RequestResponse")]
        public string RequestId { get; set; }

        public RequestResponse RequestResponse { get; set; }
        #endregion
    }
}
