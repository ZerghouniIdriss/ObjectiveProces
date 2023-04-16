using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ListOneOnOneSessionViewModel
    {
        public int Id { get; set; }

        public AssignToViewModel Attendee { get; set; }
        public AssignToViewModel Animator { get; set; }

        public OneOnOneSessionStatusViewModel SessionStatus { get; set; }
        public string Note { get; set; }

    }
}