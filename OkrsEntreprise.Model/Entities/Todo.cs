using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class Todo : EntityBase
    {
         
            public string Description { get; set; }
            public bool IsDone { get; set; }

            [ForeignKey("KeyResultId")]
            public virtual KeyResult KeyResult { get; set; }
            public long KeyResultId { get; set; }
        }
     
}
