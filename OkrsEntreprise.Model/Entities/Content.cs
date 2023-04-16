using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class Content : EntityBase
    { 
            public string Code { get; set; }
            public string Text { get; set; }

    }
     
}
