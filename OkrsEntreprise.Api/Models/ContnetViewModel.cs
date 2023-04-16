using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ContentViewModel
    {
        public long id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string code { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string text { get; set; }
 
    }
}