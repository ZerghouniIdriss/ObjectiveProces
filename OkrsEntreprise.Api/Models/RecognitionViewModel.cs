using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class RecognitionViewModel
    {
        public long id { get; set; }

        public RecognitionReceiverViewModel giver { get; set; }

        public string text { get; set; }

        public RecognitionReceiverViewModel[] receivers { get; set; }

        public RecognitionTeamReceiverViewModel[] teamReceivers { get; set; }

        public RecognitionGoalViewModel goal { get; set; }

    }

    public class RecognitionReceiverViewModel
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

    }

    public class RecognitionTeamReceiverViewModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public string description { get; set; }
        public string Avatar { get; set; }

    }

    public class RecognitionGoalViewModel
    {
        public long id { get; set; }

        public string title { get; set; }
    }

    public class CreateRecognitionViewModelViewModel
    {
        public string text { get; set; }
        public long goal { get; set; }
        public long[] teams { get; set; }
        public long[] users { get; set; }
    }
}