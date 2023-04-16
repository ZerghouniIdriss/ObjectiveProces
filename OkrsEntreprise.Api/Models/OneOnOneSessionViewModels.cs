using System;
using System.Collections;
using System.Collections.Generic;
using OkrsEntreprise.Model.Entities;
using System.ComponentModel.DataAnnotations;
using OkrsEntreprise.Api.CustomClass;

namespace OkrsEntreprise.Api.Models
{

    public class OneOnOneSessionViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } 
        [Required]
        public AssignToViewModel Attendee { get; set; }
        public AssignToViewModel Animator { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "At least a goal is required in the session")]
        public SessionGoalViewModel[] Goals { get; set; }
        public OneOnOneSessionStatusViewModel SessionStatus { get; set; }
        public string Note { get; set; }
    }


    public class OneOnOneSessionStatusViewModel
    {
        public int id { get; set; }
        public string title { get; set; }

    }

}