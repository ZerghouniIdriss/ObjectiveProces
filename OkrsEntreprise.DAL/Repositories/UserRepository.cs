using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Associations;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IUserRepository : IUserStore<ApplicationUser, long>
    {
        void AddTeamsToUser(ApplicationUser user, Team[] teams);

        void RemoveTeamToUser(Team team, ApplicationUser user);

        void AddRoleToUser(ApplicationUser user, ApplicationRole role);

        void RemoveRoleFromUser(ApplicationRole applicationRole, ApplicationUser user);


        void EditTenant(ApplicationUser user);

        Task<List<ApplicationUser>> GetMostRecognized(int top);
    }

    public class UserRepository : UserStore<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserRepository
    {
        private IOkrsContext _context = null;
        public UserRepository(IOkrsContext context)
            : base(context as DbContext)
        {
            _context = context;
        }

        public UserRepository()
            : base(new OkrsContext())
        {
            _context = new OkrsContext();
        }

        public void AddTeamsToUser(ApplicationUser user, Team[] teams)
        {
            using (var context = new OkrsContext())
            {
                ApplicationUser gObj = new ApplicationUser() { Id = user.Id };

                context.Users.Add(gObj);
                context.Users.Attach(gObj);

                foreach (var team in teams)
                {
                    context.Teams.Add(team);
                    context.Teams.Attach(team);

                    gObj.Teams.Add(team);
                }
                context.SaveChanges();
            }

        }





        public void RemoveTeamToUser(Team team, ApplicationUser user)
        {
            using (var context = new OkrsContext())
            {
                Team tObj = context.Teams.FirstOrDefault(g => g.Id == team.Id);

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

        public void AddRoleToUser(ApplicationUser user, ApplicationRole role)
        {
            using (var context = new OkrsContext())
            {
                ApplicationUser uObj = new ApplicationUser() { Id = user.Id };

                context.Users.Add(uObj);
                context.Users.Attach(uObj);

                //context.Roles.Add(role);
                //context.Roles.Attach(role);

                uObj.Roles.Add(new ApplicationUserRole() { UserId = uObj.Id, RoleId = role.Id });

                context.SaveChanges();
            }
        }

        public void RemoveRoleFromUser(ApplicationRole applicationRole, ApplicationUser user)
        {
            using (var context = new OkrsContext())
            {
                var usr = context.Users.FirstOrDefault(u => u.Id == user.Id);

                if (usr != null)
                {
                    var roleToRemove = usr.Roles.FirstOrDefault(r => r.RoleId == applicationRole.Id);

                    if (roleToRemove != null)
                    {
                        usr.Roles.Remove(roleToRemove);
                    }
                }
                // call SaveChanges
                SaveContextChange(context);
            }

        }

        public void EditTenant(ApplicationUser user)
        {
            using (var context = new OkrsContext())
            {
                var userToUpdate = context.Users.FirstOrDefault(u => u.UserName == user.UserName);

                context.Set<ApplicationUser>().Attach(userToUpdate);

                userToUpdate.TenantId = user.TenantId;
                context.Entry(userToUpdate).Property(x => x.TenantId).IsModified = true;

                SaveContextChange(context);
            }
        }


        public Task<List<ApplicationUser>> GetMostRecognized(int top)
        {
            return Task.Run(() =>
            {
                using (var context = new OkrsContext())
                {
                    return context
                        .Users
                        .Include(x => x.Recognitions)
                        .Include(x => x.Recognitions.Select(r => r.Goal))
                        .Include(x => x.Recognitions.Select(r => r.Giver))
                        .OrderByDescending(u => u.Recognitions.Count).Take(top).ToList();
                }
            });
        }

        private void SaveContextChange(OkrsContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }
        }

    }
}
