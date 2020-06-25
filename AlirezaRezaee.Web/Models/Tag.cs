using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
    }
}
