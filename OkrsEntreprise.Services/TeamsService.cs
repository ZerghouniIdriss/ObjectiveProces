using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface ITeamService
    {

        void Add(Team Team);
        void Update(params Team[] departments);
        void Remove(params Team[] departments);

        void AddTeam(Team team);

        void EditName(Team team);

        void EditDescription(Team team);

        void EditAvatar(Team team);

        void AddUserToTeam(Team team, ApplicationUser[] users);

        void RemoveUserFromTeam(ApplicationUser user, Team team);

        Team GetById(long id);
        IList<Team> GetAll();

        IList<ApplicationUser> GetUsersForTeams(long[] teams);
    }

    public class TeamService : CRUDServiceBase<TeamRepository, Team>, ITeamService
    {
        private ITeamRepository _teamRepository;
        private IUserRepository _userRepository;

        public TeamService(ITeamRepository TeamRepository, IUserRepository userRepository)
        {
            this._teamRepository = TeamRepository;
            this._userRepository = userRepository;
        }

        public Team GetById(long id)
        {
            return _teamRepository.GetSingle(x => x.Id == id, x => x.Users, x => x.Goals, x => x.Goals.Select(g => g.KeyResults),
                x => x.Goals.Select(g => g.GoalStatus),
                x => x.Recognitions, x => x.Recognitions.Select(r => r.Giver), 
                x => x.Recognitions.Select(r => r.Goal),
                x => x.Recognitions.Select(r => r.Receivers));
        }

        public IList<Team> GetAll()
        {
            return _teamRepository.GetAll(x => x.Users, x => x.Goals, x => x.Goals.Select(g => g.GoalStatus), x => x.Goals.Select(g => g.KeyResults), x => x.Recognitions, x => x.Recognitions.Select(r => r.Giver), x => x.Recognitions.Select(r => r.Goal));
        }

        public IList<ApplicationUser> GetUsersForTeams(long[] teams)
        {
            return _teamRepository.GetUsersForTeams(teams);
        }

        public void EditName(Team team)
        {
            Team toUpdate = _teamRepository.GetSingle(u => u.Id == team.Id);

            if (toUpdate != null)
            {
                toUpdate.Name = team.Name;
                _teamRepository.Update(toUpdate, x => x.Name);
            }
            else
            {
                throw new Exception("Team not found");
            }
        }

        public void EditDescription(Team team)
        {
            Team toUpdate = _teamRepository.GetSingle(u => u.Id == team.Id);

            if (toUpdate != null)
            {
                toUpdate.Description = team.Description;
                _teamRepository.Update(toUpdate, x => x.Description);
            }
            else
            {
                throw new Exception("Team not found");
            }
        }

        public void EditAvatar(Team team)
        {
            Team toUpdate = _teamRepository.GetSingle(u => u.Id == team.Id);

            if (toUpdate != null)
            {
                toUpdate.Avatar = team.Avatar;
                _teamRepository.Update(toUpdate, x => x.Avatar);
            }
            else
            {
                throw new Exception("Team not found");
            }
        }

        public void RemoveUserFromTeam(ApplicationUser user, Team team)
        {
            _teamRepository.RemoveUserFromTeam(team, user);
        }

        public void AddUserToTeam(Team team, ApplicationUser[] users)
        {
            _teamRepository.AddUserToTeam(team, users);
        }

        public void AddTeam(Team team)
        {
            Team toInsert = new Team() { Name = team.Name, Description = team.Description };
            _teamRepository.Add(toInsert);

            _teamRepository.AddUserToTeam(toInsert, team.Users.ToArray());
        }
    }
}
