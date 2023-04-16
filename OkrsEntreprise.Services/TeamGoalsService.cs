using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.Framework.Exceptions;

namespace OkrsEntreprise.Services
{
    public interface ITeamGoalsService
    {
        void AddGoalToTeams(Goal goal, Team[] teams);

        void RemoveGoalForTeam(Goal goal, Team team);
        IList<Goal> GoalByTeamId(long Id);

        int GetAlignedGoalByTeam(long id);
        int GetOverallProgressByTeam(long id);

    }

    public class TeamGoalsService : ITeamGoalsService
    {
        private IGoalRepository _goalRepository;
        private ITeamRepository _teamRepository;
        private IActivityService _activityService;

        public TeamGoalsService(IGoalRepository goalRepository, ITeamRepository teamRepository, IActivityService activityService)
        {
            this._goalRepository = goalRepository;
            this._teamRepository = teamRepository;
            this._activityService = activityService;
        }


        public void AddGoalToTeams(Goal goal, Team[] teams)
        {
            _goalRepository.AddGoalToTeams(goal, teams);
            //_activityService.Add("Assigned", goal.ToString(), teams.Select(t => t.Name).ToArray());
        }

        public void RemoveGoalForTeam(Goal goal, Team team)
        {
            _goalRepository.RemoveGoalForTeam(goal, team);

        }

        public IList<Goal> GoalByTeamId(long Id)
        {
            return _goalRepository.GetList(x =>
            {
                var firstOrDefault = x.Teams.FirstOrDefault();
                return firstOrDefault != null && firstOrDefault.Id == Id;
            }, x => x.KeyResults, x => x.GoalStatus,x=>x.Users, x=>x.Teams, 
               x => x.GoalCategory, x => x.EntityCreator,x=> x.Recognitions, x => x.Parent, x => x.SubGoals);
        } 
             

        public int GetOverallProgressByTeam(long id)
        {
            var allGoals = _goalRepository.GetAllByTeamId(id);
            var countGoals = allGoals.Count;

            if (countGoals == 0)
                return 0;

            var totalProgress = allGoals.Sum(g => g.Progress);

            var overallProgress = (int)((double)totalProgress / countGoals);

            if (overallProgress < 0 || overallProgress > 100)
            {
                throw new OutOfExpectedRangeException("overallProgress", 0.ToString(), 100.ToString());
            }

            return overallProgress;
        }

        public int GetAlignedGoalByTeam(long id)
        {
            var countAllAlignedGoals = _goalRepository.GetAllAlignedGoalByTeamId(id).Count;
            if (countAllAlignedGoals == 0)
                return 0;

            var allGoals = _goalRepository.GetAllOpenByUserId(id).Count;
            if (allGoals == 0)
                return 0;


            var alignedGoal = (int)((double)countAllAlignedGoals / allGoals * 100);

            if (alignedGoal < 0 || alignedGoal > 100)
            {
                throw new OutOfExpectedRangeException("overallProgress", 0.ToString(), 100.ToString());
            }

            return alignedGoal;
        }

    }
}
