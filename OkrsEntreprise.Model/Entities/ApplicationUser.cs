using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Entities
{
    [TenantAware("TenantId")]
    public class ApplicationUser : IdentityUser<long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<long>
    {
        public ApplicationUser()
        {
            CreationdDate = DateTime.Now;
            Goals = new HashSet<Goal>();
            Teams = new HashSet<Team>();
            OneOnOneSessions = new HashSet<OneOnOneSession>();

        }

        public DateTime CreationdDate { get; set; }

        //public ApplicationUser CreatorUser { get; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public string Avatar { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }

        public virtual ICollection<OneOnOneSession> OneOnOneSessions { get; set; }


        public Nullable<long> ManagerId { get; set; }
        public virtual ApplicationUser Manager { get; set; }
        public virtual ICollection<ApplicationUser> Staff { get; set; }


        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }


        public virtual ICollection<Recognition> Recognitions { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager, string authenticationType)
        {
            var userIdentity = new ClaimsIdentity();
            try
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("TenantId", TenantId.ToString()));

            return userIdentity;
        }

    }

}