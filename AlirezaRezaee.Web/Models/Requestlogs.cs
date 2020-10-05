using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Requestlogs
    {
        public long Id { get; set; }

        public string RequestId { get; set; }

        public string Method { get; set; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public int StatusCode { get; set; }

        public string Protocol { get; set; }

        public long? ResponseContentLength { get; set; }

        public string Referrer { get; set; }

        public string IP { get; set; }

        public string FilesPath { get; set; }

        public string RequestBodyFilePathPostfix { get; set; }

        public string RequestHeadersFilePathPostfix { get; set; }

        public string ResponseBodyFilePathPostfix { get; set; }

        public string ResponseHeadersFilePathPostfix { get; set; }

        public DateTime Time { get; set; }
    }
}
