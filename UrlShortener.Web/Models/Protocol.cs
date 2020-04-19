using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Web.Models
{
    public class Protocol
    {
        public int Id { get; set; }

        public string ProtocolName { get; set; }

        #region Relations
        public ICollection<Url> Urls { get; set; }

        public ICollection<CustomUrl> CustomUrls { get; set; }
        #endregion
    }
}
