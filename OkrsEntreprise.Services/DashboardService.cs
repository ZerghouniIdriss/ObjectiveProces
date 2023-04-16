using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OkrsEntreprise.Services
{
    public interface IDashboardService
    {
        IList<Goal> GetOnTrackGoals(IList<Goal> goals);
        IList<Goal> GetAtRiskGoals(IList<Goal> goals);
        IList<Goal> GetDelayedGoals(IList<Goal> goals);
        int GetOverallProgress();
        IList<KeyValuePair<string, int>> GetOrksByProgress();
        int GetOverallProgressByUser(ApplicationUser userId);
    }

    public class DashboardService : CRUDServiceBase<GoalRepository, Goal>, IDashboardService
    {
        private IGoalRepository goalRepository;

        public DashboardService(IGoalRepository goalRepository)
        {
            this.goalRepository = goalRepository;
        }

        public IList<Goal> GetOnTrackGoals(IList<Goal> goals)
        {
            return goals.Where(g => ((!g.Deadline.HasValue) || (g.Deadline.HasValue && g.Deadline > DateTime.Now.AddDays(7)))).ToList();
        }
        public IList<Goal> GetAtRiskGoals(IList<Goal> goals)
        {
            return goals.Where(g => (g.Deadline.HasValue && g.Deadline < DateTime.Now.AddDays(7) && g.Deadline > DateTime.Now)).ToList();
        }

        public IList<Goal> GetDelayedGoals(IList<Goal> goals)
        {
            return goals.Where(g => (g.Deadline.HasValue && g.Deadline < DateTime.Now)).ToList();
        }

        public int GetOverallProgress()
        {
            var goals = goalRepository.GetAllOpenAndPublic();

            if (goals.Count == 0)
                return 0;
            var totalProgress = goals.Sum(g => g.Progress);
            return (int)((double)totalProgress / goals.Count);
        }

        public IList<KeyValuePair<string, int>> GetOrksByProgress()
        {
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            var goals = GetAllGoals();
            if (goals.Count == 0)
                return result;
            var totalProgress = goals.Sum(g => g.Progress);
            var Group1Pecent = (int)((double)goals.Where(g => g.Progress <= 20).Sum(g => g.Progress) / totalProgress * 100);
            result.Add(new KeyValuePair<string, int>("0-20%", Group1Pecent));
            var Group2Pecent = (int)((double)goals.Where(g => 20 < g.Progress && g.Progress <= 40).Sum(g => g.Progress) / totalProgress * 100);
            result.Add(new KeyValuePair<string, int>("20-40%", Group2Pecent));
            var Group3Pecent = (int)((double)goals.Where(g => 40 < g.Progress && g.Progress <= 60).Sum(g => g.Progress) / totalProgress * 100);
            result.Add(new KeyValuePair<string, int>("40-60%", Group3Pecent));
            var Group4Pecent = (int)((double)goals.Where(g => 60 < g.Progress && g.Progress <= 80).Sum(g => g.Progress) / totalProgress * 100);
            result.Add(new KeyValuePair<string, int>("60-80%", Group4Pecent));
            var Group5Pecent = (int)((double)goals.Where(g => 80 < g.Progress && g.Progress <= 100).Sum(g => g.Progress) / totalProgress * 100);
            result.Add(new KeyValuePair<string, int>("80-100%", Group5Pecent));
            return result;
        }
        public int GetOverallProgressByUser(ApplicationUser user)
        {
            var goals = user.Goals.Where(g => g.IsOpen).ToList();
            var goalsCount = goals.Count;
            if (goalsCount == 0)
                return 0;
            var totalProgress = goals.Sum(g => g.Progress);
            return (int)((double)totalProgress / goalsCount);
        }

        public IList<Goal> GetAllGoals()
        {
            return goalRepository.GetAll(x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator,
                x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
                x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams, x => x.Recognitions)
                .Where(x => x.IsOpen && !x.IsPrivate).ToList();
        }
    }
}
