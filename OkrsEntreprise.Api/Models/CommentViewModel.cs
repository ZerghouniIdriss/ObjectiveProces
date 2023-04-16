using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class CommentViewModel
    {
        public long id { get; set; }

        [Required(ErrorMessage = "Goal is required")]
        public long goalid { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        public string text { get; set; }

        public DateTime createddate { get; set; }

        public AssignToViewModel user { get; set; }
    }
}