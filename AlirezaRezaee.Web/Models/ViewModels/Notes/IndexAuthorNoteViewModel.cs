using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Models.ViewModels.Notes
{
    public class IndexAuthorNoteViewModel
    {
        public IEnumerable<AuthorNoteModel> AuthorNotes { get; set; }

        public AuthorNoteModel SingleNote { get; set; }
    }
}
