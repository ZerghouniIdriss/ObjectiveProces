using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class GoalCategory: EntityBase
    {
        public GoalCategory()
        {
            Goals = new HashSet<Goal>();
        }

        public string CategoryTitle { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public override string ToString()
        {
            return "#" + Id + "-" + CategoryTitle;
        }

    }
}
