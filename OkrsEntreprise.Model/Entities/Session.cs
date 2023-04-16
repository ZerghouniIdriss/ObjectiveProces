using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class Session : EntityBase
    {
        [ForeignKey("SessionStatusId")]
        public virtual SessionStatus SessionStatusStatus { get; set; }
        public long SessionStatusId { get; set; }
    }
}
