using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    [TenantAware("TenantId")]
    public class Recognition : EntityBase
    {
        public Recognition()
        {
            Receivers = new HashSet<ApplicationUser>();
            TeamReceivers = new HashSet<Team>();
        }


        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }

        [ForeignKey("GiverId")]
        public virtual ApplicationUser Giver { get; set; }
        public long GiverId { get; set; }

        public string Text { get; set; }

        public virtual ICollection<ApplicationUser> Receivers { get; set; }
        public virtual ICollection<Team> TeamReceivers { get; set; }

        [ForeignKey("GoalId")]
        public virtual Goal Goal { get; set; }
        public long GoalId { get; set; }       
    }
}

