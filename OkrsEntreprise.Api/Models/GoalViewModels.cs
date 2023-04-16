using OkrsEntreprise.Api.CustomClass;
using System;
using System.ComponentModel.DataAnnotations;

namespace OkrsEntreprise.Api.Models
{
    public class GoalFullLoadViewModel
    {

        public long id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must be at most 100 characters long.")]
        public string title { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "At least a person is required")]
        public AssignToViewModel[] assignees { get; set; }


        public AssignToViewModel[] users { get; set; }

        public AssignToViewModel[] teams { get; set; }

        public GoalCategoryViewModel category { get; set; }

        public bool isprivate { get; set; }

        public bool isopen { get; set; }

        public string description { get; set; }
        public long? ParentId { get; set; }

        public int priority { get; set; }

        public long progress { get; set; }

        public DateTime? duedate { get; set; }

        public GoalStatusViewModel status { get; set; }

        public KeyResultViewModel[] keyresults { get; set; }

        public DateTime createddate { get; set; }

        public CommentViewModel[] comments { get; set; }

        public ParentChildGoalViewModel parent { get; set; }

        public ParentChildGoalViewModel[] children { get; set; }

        public RecognitionViewModel[] recognitions { get; set; }

        public GoalIndicatorViewModel[] GoalIndicators { get; set; }
    }


    public class GoalViewModel
    {
        public long id { get; set; }
        public string title { get; set; }


        public DateTime? duedate { get; set; }

        public string description { get; set; }

        public bool isprivate { get; set; }

        public int progress { get; set; }
        public bool isaligned { get; set; }

        public int priority { get; set; }

        public bool isopen { get; set; }

        public bool hasRecognition { get; set; }
    }

}