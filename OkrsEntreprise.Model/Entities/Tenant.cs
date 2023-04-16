using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OkrsEntreprise.Model.Entities
{
    public class Tenant 
    {
        [Key]
        public long Id { get; set; }
        public Tenant()
        {
            Users = new HashSet<ApplicationUser>();
            Goals = new HashSet<Goal>();
        }

        public virtual string Name { get; set; }


        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public override string ToString()
        {
            return "#" + Id + "-" + Name;
        }

    }
}