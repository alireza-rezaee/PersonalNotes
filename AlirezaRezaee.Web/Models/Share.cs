using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class Share : Post
    {
        public int ShareId { get; set; }

        [Display(Name = "نشانی پیوند")]
        public string Url { get; set; }

        public ICollection<ShareCategory> ShareCategories { get; set; }
    }
}
