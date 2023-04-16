using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class OneOnOneSession :EntityBase
    {
        public OneOnOneSession()
        {
           Goals = new HashSet<Goal>();

        }
        [ForeignKey("AttendeeId")]
        public virtual ApplicationUser Attendee { get; set; }
        public long AttendeeId { get; set; }

        [ForeignKey("AnimatorId")]
        public virtual ApplicationUser Animator { get; set; }
        public long  AnimatorId { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }


        [ForeignKey("SessionStatusId")]
        public virtual SessionStatus SessionStatus { get; set; }
        public long SessionStatusId { get; set; }

        public virtual string Note { get; set; }

    }
}
