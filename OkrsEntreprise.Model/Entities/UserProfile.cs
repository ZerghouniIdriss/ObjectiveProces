using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    public class UserProfile: EntityBase
    { 
        public DateTime? BirhtDay { get; set; }

        public bool Gender { get; set; }

        public string Address { get; set; }
          
        public long? ContactNumber { get; set; }
        
        public string ProfilePictureUrl { get; set; }

        public long UserId { get; set; }

        //public virtual User User { get; set; }
        
    }
}
