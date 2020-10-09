using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models.JsonModels
{
    public class RequestFormLog
    {
        public List<StringCouple> FormFields { get; set; }

        public List<FileDetails> FormFiles { get; set; }
    }

    public class FileDetails
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Length { get; set; }
    }
}
