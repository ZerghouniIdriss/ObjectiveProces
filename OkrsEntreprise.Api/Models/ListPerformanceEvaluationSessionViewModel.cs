using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ListPerformanceEvaluationSessionViewModel
    {
        public int Id { get; set; }
        public AssignToViewModel Attendee { get; set; }
        public AssignToViewModel Animator { get; set; }

        public PerformanceEvaluationSessionStatusViewModel SessionStatus { get; set; }
        public string AttendeeNote { get; set; }

        public string AnimatorNote { get; set; }
    }
}