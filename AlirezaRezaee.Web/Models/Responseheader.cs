using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Responseheader : StringCouple
    {
        #region Relations
        [Key]
        [ForeignKey("Requestlogs")]
        public int RequestlogId { get; set; }

        public Requestlogs Requestlogs { get; set; }
        #endregion
    }
}
