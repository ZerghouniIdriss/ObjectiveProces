using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
     
    public class EntityBase   
    {   

        protected EntityBase()
        {
            CreatedDate = DateTime.Now; 
        }

        [Key] 
        public long Id { get; set; }

        public virtual  DateTime CreatedDate { get; set; }
        
        [ForeignKey("EntityCreatorId")]
        public virtual ApplicationUser EntityCreator { get; set; }
        public long? EntityCreatorId { get; set; }


    }
 
}
