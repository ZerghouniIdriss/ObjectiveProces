using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ParentChildGoalViewModel
    {
        public long parentid { get; set; }

        public long id { get; set; }
        public string title { get; set; }
    }
}