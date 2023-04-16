using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities.Surveys
{
    public abstract class SurveyUserAnswer :EntityBase
    {
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        public long SurveyId { get; set; }

        [ForeignKey("SurveyQuestionId")]
        public virtual SurveyQuestion SurveyQuestion { get; set; }
        public long SurveyQuestionId { get; set; }

        [ForeignKey("SurveyOptionId")]
        public virtual SurveyOption SurveyOption { get; set; }
        public long SurveyOptionId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public long UserId { get; set; }
         
    }
}