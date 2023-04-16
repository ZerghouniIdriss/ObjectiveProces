using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{

    [TenantAware("TenantId")]
    public class Team:  EntityBase
    {
        public Team()
        { 
            Users = new HashSet<ApplicationUser>();

            Goals = new HashSet<Goal>();
        }

        public string Name { set; get; }

        public string Description { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }

        public string Avatar { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        
        public virtual ICollection<Goal> Goals { get; set; }

        public virtual ICollection<Recognition> Recognitions { get; set; }

    }
}