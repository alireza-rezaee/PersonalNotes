using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Recommendeds
    {
        [Display(Name = "رتبه")]
        public int Rank { get; set; }

        #region Relations
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        public Post Post { get; set; }
        #endregion
    }
}
