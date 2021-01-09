using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Models
{
    public class Personalization
    {
        [Key]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
