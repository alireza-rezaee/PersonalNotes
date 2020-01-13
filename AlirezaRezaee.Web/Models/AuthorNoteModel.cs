using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models
{
    public class AuthorNoteModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Content { get; set; }
    }
}
