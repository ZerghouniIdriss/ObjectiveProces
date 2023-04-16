using System.Collections.Generic;
using OkrsEntreprise.Model.Entities;
using System.Linq;
using OkrsEntreprise.DAL.Context;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface ITeamRepository : IRepositoryBase<Team>
    {
        IList<ApplicationUser> GetUsersForTeams(long[] teams);

        void AddUserToTeam(Team team, ApplicationUser[] users);

        void RemoveUserFromTeam(Team team, ApplicationUser user);
    }

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public IList<ApplicationUser> GetUsersForTeams(long[] teams)
        {
            return this.GetList(t => teams.Contains(t.Id), x => x.Users).SelectMany(t => t.Users).ToList();

        }


        public void AddUserToTeam(Team team, ApplicationUser[] users)
        {
            using (var context = new OkrsContext())
            {
                context.Teams.Add(team);
                context.Teams.Attach(team);

                foreach (var user in users)
                {
                    context.Users.Add(user);
                    context.Users.Attach(user);

                    team.Users.Add(user);
                }

                context.SaveChanges();
            }

        }

        public void RemoveUserFromTeam(Team team, ApplicationUser user)
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

    }
}
