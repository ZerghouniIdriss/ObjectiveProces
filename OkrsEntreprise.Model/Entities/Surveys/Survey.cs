using System.Collections.Generic;

namespace OkrsEntreprise.Model.Entities.Surveys
{
	public class Survey : EntityBase
	{
	    public Survey()
	    {
	        Questions =  new HashSet<SurveyQuestion>();
            Options = new HashSet<SurveyOption>();
        }

        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SurveyQuestion> Questions { get; set; }

        public virtual ICollection<SurveyOption> Options { get; set; }
       
    }
}