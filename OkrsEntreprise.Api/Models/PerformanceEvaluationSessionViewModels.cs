using OkrsEntreprise.Api.CustomClass;
using System;
using System.ComponentModel.DataAnnotations;

namespace OkrsEntreprise.Api.Models
{

    public class PerformanceEvaluationSessionViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public AssignToViewModel Attendee { get; set; }
        public AssignToViewModel Animator { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "At least a goal is required in the session")]
        public SessionGoalViewModel[] Goals { get; set; }

        public PerformanceEvaluationSessionStatusViewModel SessionStatus { get; set; }
        public string AttendeeNote { get; set; }

        public string AnimatorNote { get; set; }

        public string PerformanceSurveyId { get; set; }

        public SurveyViewModel PerformanceSurvey { get; set; }

        public PerformanceSurveyUserAnswerViewModel[] Anwers { get; set; }
    }


    public class PerformanceEvaluationSessionStatusViewModel
    {
        public int id { get; set; }
        public string title { get; set; }

    }


    public class PerformanceSurveyUserAnswerViewModel
    {
        public long surveyquestionid { get; set; }
        public long surveyoptionid { get; set; }

        public string comment { get; set; }
    }

}