using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models
{
    public class Share
    {
        [Display(Name = "نشانی پیوند")]
        public string RedirectToUrl { get; set; }

        #region Relations
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
        #endregion
    }
}
