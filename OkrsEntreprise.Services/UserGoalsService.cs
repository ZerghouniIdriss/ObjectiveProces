using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Framework;
using OkrsEntreprise.Framework.Exceptions;
using OkrsEntreprise.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OkrsEntreprise.Services
{
    public interface IUserGoalsService
    {
        IList<Goal> GoalByUserId(long id);
        IList<Goal> GoalByUserName(string userName);
        void AddGoalToUser(Goal goal, ApplicationUser[] currentUser);

        void RemoveGoalToUser(Goal goal, ApplicationUser currentUser);

        int GetOverallProgressByUser(long id);
        int GetAlignedGoalByUser(long id);
    }

    public class UserGoalsService : IUserGoalsService
    {
        private IGoalRepository _goalRepository;
        private IGoalRepository _userRepository;
        private IActivityService _activityService;

        private IEmailService _emailService;

        private ICurrentContextProvider<ApplicationUser> _currentContextProvider;


        public UserGoalsService(IGoalRepository goalRepository, IActivityService activityService, ICurrentContextProvider<ApplicationUser> currentContextProvider, IEmailService emailService)
        {
            _goalRepository = goalRepository;
            _activityService = activityService;
            _currentContextProvider = currentContextProvider;
            _emailService = emailService;
        }

        public IList<Goal> GoalByUserId(long Id)
        {
            return _goalRepository.GetList(x =>
            {
                var firstOrDefault = x.Users.FirstOrDefault();
                return firstOrDefault != null && firstOrDefault.Id == Id;
            }, x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator);
        }

        public IList<Goal> GoalByUserName(string userNmae)
        {
            return _goalRepository.GetList(x =>
            {
                var firstOrDefault = x.Users.FirstOrDefault();
                return firstOrDefault != null && firstOrDefault.UserName == userNmae;
            }, x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator);
        }

        public void AddGoalToUser(Goal goal, ApplicationUser[] users)
        {
            _goalRepository.AddGoalToUser(goal, users);
            var createdGoal = _goalRepository.GetSingle(g=>g.Id== goal.Id, g=>g.EntityCreator,g=>g.Users);
             
            foreach (var recepient in createdGoal.Users)
            {
                if (!string.IsNullOrEmpty(recepient.Email))
                {
                    _emailService.SendEmail(recepient.Email,"You are assigned to new Objective", _currentContextProvider.GetCurrentUser().UserName + " Assigned you to " + goal.ToString());
                }

            }

        }

        public void RemoveGoalToUser(Goal goal, ApplicationUser currentUser)
        {
            _goalRepository.RemoveGoalToUser(goal, currentUser);
            // _activityService.Add("Unassigned", goal.ToString(), currentUser.UserName); 
        }

        public int GetOverallProgressByUser(long id)
        {
            var allGoals = _goalRepository.GetAllOpenByUserId(id);
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



        public int GetAlignedGoalByUser(long id)
        {
            var countAllAlignedGoals = _goalRepository.GetAllAlignedGoalByUserId(id).Count;
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
