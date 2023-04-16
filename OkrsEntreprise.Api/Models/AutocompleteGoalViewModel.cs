using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class AutocompleteGoalViewModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public bool isprivate { get; set; } 
        public bool isopen { get; set; }
    }
}