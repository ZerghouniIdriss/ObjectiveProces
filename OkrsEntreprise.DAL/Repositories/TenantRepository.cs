using System.Collections.Generic;
using OkrsEntreprise.Model.Entities;
using System.Linq;
using OkrsEntreprise.DAL.Context;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface ITenantRepository : IRepositoryBase<Tenant>
    {
        IList<ApplicationUser> GetUsersForTeams(long[] teams);

        void AddUserToTenant(Tenant tenant, ApplicationUser[] users);

        void RemoveUserFromTenant(Tenant tenant, ApplicationUser user);
        // Tenant AddTenant(Tenant tenant);
        Tenant AddTenantWithReturnValue(Tenant tenant);
        Tenant GetTenatIdByCompanyName(string CompanyName);
    }

    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
    {
        public IList<ApplicationUser> GetUsersForTeams(long[] teams)
        {
            return this.GetList(t => teams.Contains(t.Id), x => x.Users).SelectMany(t => t.Users).ToList();

        }


        public void AddUserToTenant(Tenant tenant, ApplicationUser[] users)
        {
            using (var context = new OkrsContext())
            {
                context.Tenants.Add(tenant);
                context.Tenants.Attach(tenant);

                foreach (var user in users)
                {
                    context.Users.Add(user);
                    context.Users.Attach(user);

                    tenant.Users.Add(user);
                }

                context.SaveChanges();
            }

        }

        public void RemoveUserFromTenant(Tenant tenant, ApplicationUser user)
        {
            using (var context = new OkrsContext())
            {
                Tenant tObj = context.Tenants.FirstOrDefault(g => g.Id == tenant.Id);

                if (tObj != null)
                {
                    var usr = tObj.Users.FirstOrDefault(u => u.Id == user.Id);

                    if (usr != null)
                    {
                        tObj.Users.Remove(usr);
                    }

                    // call SaveChanges
                    SaveContextChange(context);
                }
            }
        }



        public Tenant AddTenantWithReturnValue(Tenant tenant)
        {
            using (var context = new OkrsContext())
            {
                context.Tenants.Add(tenant);
                SaveContextChange(context);

            }
            return tenant;
        }

        public Tenant GetTenatIdByCompanyName(string CompanyName)
        {
            using (var context = new OkrsContext())
            {
                Tenant tObj = context.Tenants.FirstOrDefault(g => g.Name == CompanyName.Trim());
                return tObj;

            }
        }

    }
}
