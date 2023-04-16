using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class SurveyOptionViewModel
    {
        public long id { get; set; }

        public string code { get; set; }

        public string text { get; set; }

        public int order { get; set; }

        public long surveyid { get; set; }

    }
}