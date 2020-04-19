using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Web.Models
{
    public class Url
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public bool IsEnabled { get; set; }

        #region Relations
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public Protocol Protocol { get; set; }
        #endregion
    }
}
