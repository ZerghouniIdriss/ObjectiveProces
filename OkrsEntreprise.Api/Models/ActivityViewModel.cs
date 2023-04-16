using System;

namespace OkrsEntreprise.Api.Models
{
    public class ActivityViewModel
    {
        public string Avatar { get; set; }
        public string Actor { get; set; }

        public string TargetObject { get; set; }

        public string ActionText { get; set; }
        public string BodyContent
        {
            get { return ActionText + " " + TargetObject; }
        }
        public string BodyContentWithActor
        {
            get { return Actor + " " + ActionText + " " + TargetObject; }
        }
        public DateTime ActivityDate { get; set; }
    }
}