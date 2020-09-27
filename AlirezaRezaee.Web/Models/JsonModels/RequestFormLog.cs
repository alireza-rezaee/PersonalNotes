using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.JsonModels
{
    public class RequestFormLog
    {
        public List<StringCouple> FormField { get; set; }

        public List<FileDetails> FormFile { get; set; }
    }

    public class FileDetails
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Length { get; set; }
    }
}
