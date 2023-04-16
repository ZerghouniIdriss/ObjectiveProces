using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities.Surveys
{
    public class SurveyOption : EntityBase
    {
        public string Code { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        public long SurveyId { get; set; }
    }
}