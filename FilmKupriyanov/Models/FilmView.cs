using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FilmKupriyanov.Models
{
    public class FilmView
    {
        [DisplayName("Film name")]
        public String Name { get; set; }
        [DisplayName("Film description")]
        public String Description { get; set; }
        public Int32 Year { get; set; }
        [DisplayName("Film director")]
        public String Director { get; set; }
        [DisplayName("Upload poster")]
        public String ImagePath { get; set; }
        public HttpPostedFileBase Poster { get; set; }
    }
}