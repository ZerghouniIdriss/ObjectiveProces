using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Associations;
using OkrsEntreprise.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OkrsEntreprise.DAL.Repositories
{


    public interface IRoleRepository : IRoleStore<ApplicationRole, long>
    {
        List<ApplicationRole> GetAllRoles();
    }

    public class RoleRepository : RoleStore<ApplicationRole, long, ApplicationUserRole>, IRoleRepository
    {
        public RoleRepository() : base(new OkrsContext())
        {

        }

        public List<ApplicationRole> GetAllRoles()
        {
            return this.Roles.ToList();
        }
    }


}
