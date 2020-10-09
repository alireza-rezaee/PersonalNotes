using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class RequestResponse
    {
        [Key]
        [MaxLength(50)]
        public string RequestId { get; set; }

        [MaxLength(7)]
        public string Method { get; set; }

        public bool? HasHttps { get; set; }

        [MaxLength(2048)]
        public string Path { get; set; }

        [MaxLength(2048)]
        public string QueryString { get; set; }

        public int StatusCode { get; set; }

        [MaxLength(15)]
        public string IP { get; set; }

        //Milliseconds
        public long ResponseTime { get; set; }

        public DateTime Time { get; set; }

        #region Relations
        public RequestResponseDetails Details { get; set; }
        #endregion
    }
}
