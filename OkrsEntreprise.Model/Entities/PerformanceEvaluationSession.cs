using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.Model.Entities
{
    public class PerformanceEvaluationSession : EntityBase
    {
        public PerformanceEvaluationSession()
        {
           Goals = new HashSet<Goal>();
            PerformanceSurveyUserAnswers = new HashSet<PerformanceSurveyUserAnswer>();
        } 

        [ForeignKey("AttendeeId")]
        public virtual ApplicationUser Attendee { get; set; }
        public long AttendeeId { get; set; }

        [ForeignKey("AnimatorId")]
        public virtual ApplicationUser Animator { get; set; }
        public long  AnimatorId { get; set; }

        [ForeignKey("SessionStatusId")]
        public virtual SessionStatus SessionStatus { get; set; }
        public long SessionStatusId { get; set; }
        
        public virtual ICollection<Goal> Goals { get; set; }

        public virtual string AttendeeNote { get; set; }
        public virtual string AnimatorNote { get; set; }

        [ForeignKey("PerformanceSurveyId")]
        public virtual Survey PerformanceSurvey { get; set; }
        public long PerformanceSurveyId { get; set; }


        public virtual ICollection<PerformanceSurveyUserAnswer> PerformanceSurveyUserAnswers { get; set; }
    }

    public class PerformanceSurveyUserAnswer : SurveyUserAnswer
    { 
        [ForeignKey("PerformanceEvaluationSessionId")]
        public virtual PerformanceEvaluationSession PerformanceEvaluationSession { get; set; }
        public long PerformanceEvaluationSessionId { get; set; } 

        public string Comment   { get; set; }
    }
}
