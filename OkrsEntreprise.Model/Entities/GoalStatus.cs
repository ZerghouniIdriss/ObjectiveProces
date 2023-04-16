using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class GoalStatus : EntityBase
    {
        public GoalStatus()
        {
            Goals = new HashSet<Goal>();
        }
        public string StatusTitle { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public override string ToString()
        {
            return "#" + Id + "-" + StatusTitle;
        }

    }
}
