using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{

    [TenantAware("TenantId")]
    public class Activity :  EntityBase
    {
        public Activity()
        { 
        }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }

        [ForeignKey("ActorId")]
        public virtual ApplicationUser Actor { get; set; }
        public long ActorId { get; set; }

        [ForeignKey("GoalId")]
        public virtual Goal Goal { get; set; }
        public long GoalId { get; set; }

        public string ActionText { get; set; }
    }

    //public class ActivityVerb : EntityBase
    //{
    //    public ActivityVerb()
    //    {
    //    }
    //    public virtual string Text { get; set; }
    //}

    //public override string ToString()
    //{
    //var activityActor = ActivityActor.UserName;
    //var activityVerb = ActivityVerb.Text;
    //var activityObject = ActivityObject;
    //var activityTarget = ActivityTarget;
    //var preposition = "In";
    //    return activityActor + " " + activityVerb + " " + activityObject + (ActivityTarget ?? " " + preposition + activityTarget);
    //}
    //   public virtual string ActivityObject { get; set; }  

    // public virtual string ActivityTarget { get; set; }

}
