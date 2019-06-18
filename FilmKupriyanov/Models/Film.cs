using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FilmKupriyanov.Models
{
    public class Film
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DisplayName("Film name")]
        public String Name { get; set; }
        [DisplayName("Film description")]
        public String Description { get; set; }
        public Int32 Year { get; set; }
        [DisplayName("Film director")]
        public String Director { get; set; }
        [Required]
        public String CreatorUserId { get; set; }
        [DisplayName("Publisher")]
        public ApplicationUser CreatorUser { get; set; }
        public String Poster { get; set; }
    }
}