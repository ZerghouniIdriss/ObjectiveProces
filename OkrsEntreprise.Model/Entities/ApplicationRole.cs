using System.Runtime.CompilerServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OkrsEntreprise.Model.Associations;

namespace OkrsEntreprise.Model.Entities
{
    public class ApplicationRole : IdentityRole<long, ApplicationUserRole>
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base()  
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description) : base()
        {
            this.Name = name;
            this.Description = description;
        }

      
        public virtual string Description { get; set; }
    }

}