using System;

namespace OkrsEntreprise.Api.Models
{

    public class ListGoalFullLoadViewModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public AssignToViewModel[] users { get; set; }

        public AssignToViewModel[] teams { get; set; }

        public GoalCategoryViewModel category { get; set; }

        public DateTime? duedate { get; set; }
        public GoalStatusViewModel status { get; set; }

        public string description { get; set; }

        public bool isprivate { get; set; }

        public int progress { get; set; }

        public KeyResultViewModel[] keyresults { get; set; }

        public bool isaligned { get; set; }

        public int priority { get; set; }

        public bool isopen { get; set; }

        public bool hasRecognition { get; set; }
    }
}