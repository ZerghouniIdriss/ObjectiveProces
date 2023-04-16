using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class SessionGoalViewModel
    {
        public long id { get; set; }
        public string title { get; set; }

        public int progress { get; set; }

        public GoalStatusViewModel status { get; set; }
        public KeyResultViewModel[] keyresults { get; set; }
    }
}