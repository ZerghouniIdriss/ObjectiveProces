using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class TeamViewModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public string description { get; set; }
        public string Avatar { get; set; }

        public UserTeamViewModel[] Users {get; set;}

        public SessionGoalViewModel[] Goals { get; set; }

        public virtual List<ListGoalFullLoadViewModel> Objectives { get; set; }

        public TeamRecognitionViewModel[] Recognitions { get; set; }
    }

    public class TeamRecognitionViewModel
    {
        public long id { get; set; }

        public RecognitionReceiverViewModel giver { get; set; }

        public string text { get; set; }

        public RecognitionGoalViewModel goal { get; set; }

    }
}