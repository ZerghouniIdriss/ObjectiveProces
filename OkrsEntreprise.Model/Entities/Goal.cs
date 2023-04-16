using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
    [TenantAware("TenantId")]
    public class Goal :  EntityBase
    {
        public Goal()
        { 
            Users = new HashSet<ApplicationUser>();
            Teams = new HashSet<Team>();
            KeyResults = new HashSet<KeyResult>();
            Comments = new HashSet<Comment>();
            Activities = new HashSet<Activity>();
            SubGoals = new HashSet<Goal>();
            OneOnOneSessions = new HashSet<OneOnOneSession>();
            PerformanceEvaluationSessions = new HashSet<PerformanceEvaluationSession>();
            Recognitions = new HashSet<Recognition>();
        }

        public string  Title { set; get; }

        public string Detail { get; set; } 

        public bool IsPrivate { get; set; } 

        public long Progress { get; set; }

        [ForeignKey("GoalStatusId")]
        public virtual GoalStatus GoalStatus { get; set; }
        public long GoalStatusId { get; set; }

        [ForeignKey("GoalCategoryId")]
        public virtual GoalCategory GoalCategory { get; set; }
        public long GoalCategoryId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? Deadline{ get; set; }

        public virtual ICollection<KeyResult> KeyResults { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
          
        public Nullable<long> ParentId { get; set; } 
        public virtual Goal Parent { get; set; } 
        public virtual ICollection<Goal> SubGoals{ get; set; }

        public virtual bool IsOpen { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant  { get; set; }
        public long TenantId { get;  set; }

        public virtual ICollection<GoalIndicator> GoalIndicators { get; set; }

        public virtual ICollection<OneOnOneSession> OneOnOneSessions { get; set; }

        public virtual ICollection<PerformanceEvaluationSession> PerformanceEvaluationSessions { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
 
        public virtual ICollection<Recognition> Recognitions { get; set;}

        public int Priority { get; set; }
 
        public override string ToString()
        {
           return "#"+ Id + "-" + Title;
        }

    } 


}
