using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class SurveyViewModel
    {
        public long id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string code { get; set; }

        public string description { get; set; }

        public SurveyQuestionViewModel[] questions { get; set; }

        public SurveyOptionViewModel[] options { get; set; }
    }

    //public class SurveyViewModel
    //{
    //    public long id { get; set; }

    //    public string code { get; set; } 
    //}
}