using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Shares
{
    public class ShareSummary : Post
    {
        [Key]
        public int ShareId { get; set; }
        public string Url { get; set; }
    }
}
