using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Framework;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkrsEntreprise.Services
{
    public interface IGoalsService
    {
        void Add(Goal goal);
        void Update(params Goal[] goals);
        void Remove(params Goal[] goals);

        // You defined the CRUDServiceBase to do CRUD, so the controller will not execute Add method
        //but it call directly to Add funtion of Service Base !
        //add the declaration here to be executed by controller
        void AddGoal(Goal goal);
        void RemoveGoals(params Goal[] goals);
        void UpdateGoals(params Goal[] goals);

        Goal GetByGaolId(long id);
        IList<Goal> GetAllFullLoad();

        IList<Goal> GetAll();
        IList<Goal> GetAllClosedGoals();

        IList<Goal> GetAllByUserId(long id);
        IList<Goal> ByUserName(string userName);
        void AddGoalToUser(Goal goal, Model.Entities.ApplicationUser[] currentUser);
        void AddKeyResultToGoal(Goal goalToUpdate, KeyResult goal);
        void UpdateStatus(long goalId, long newStatus);
        void UpdatePriority(long goalId, int priority);
        void UpdateCategory(long goalId, long newCategory);
        void UpdateTitle(long goalId, string title);
        void UpdateProgress(long goalId, long progress);
        void UpdateIsPrivate(long goalId, bool isPrivate);
        void UpdateDueDate(long goalId, DateTime? dueDate);
        void UpdateParent(long goalId, long? parentId);
        void UpdateDes(long goalId, string des);
        void UpdateIsOpen(long goalId, bool isOpen);
        void UpdateIndicator(long goalId, long indicatorId, long indicatorValueId);
        void AddSubGoal(long parentId, long subgoalId);
        void DeleteSubGoal(long parentId, long subgoalId);

        IList<Goal> PrivateOnlyByUserId(long id);
        bool IsAligned(long id);
        IList<Goal> GetGoalsMap();
        Task<List<Goal>> MostRecognized(int top);
    }

    public class GoalsService : CRUDServiceBase<GoalRepository, Goal>, IGoalsService
    {
        private IGoalRepository goalRepository;
        private IActivityService _activityService;
        private IGoalIndicatorRepository _goalIndicatorsRepository;
        private ICurrentContextProvider<ApplicationUser> _currentContextProvider;


        public GoalsService(IGoalRepository goalRepository, IActivityService activityService, ICurrentContextProvider<ApplicationUser> currentContextProvider, IGoalIndicatorRepository goalIndicatorsRepository)
        {
            this.goalRepository = goalRepository;
            this._activityService = activityService;

            this._currentContextProvider = currentContextProvider;
            _goalIndicatorsRepository = goalIndicatorsRepository;
        }

        public Goal GetByGaolId(long id)
        {
            return goalRepository.GetSingle(x => x.Id == id, x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Users,
                x => x.Comments, x => x.Comments.Select(y => y.EntityCreator), x => x.Parent, x => x.SubGoals, x => x.Teams, x => x.Recognitions, x => x.Recognitions.Select(r => r.Giver)
                , x => x.Recognitions.Select(r => r.Receivers), x => x.Recognitions.Select(r => r.TeamReceivers), x => x.GoalIndicators.Select(i => i.Indicator), x => x.GoalIndicators.Select(i => i.IndicatorValue));
        }


        // Does not include Private and closed goals
        public IList<Goal> GetAllFullLoad()
        {
            return
                goalRepository.GetList(x => x.IsOpen && !x.IsPrivate,
                        x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator,
                        x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
                        x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams, x => x.Recognitions)
                    .OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IList<Goal> GetGoalsMap()
        {
            return goalRepository.GetGoalsMaps();
        }

        public override IList<Goal> GetAll()
        {
            return goalRepository.GetAllOpenAndPublic().OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IList<Goal> GetAllIncludingClosedGoals()
        {
            return goalRepository.GetAll(x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator,
                x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
                x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams).
                Where(x => !x.IsPrivate).
                OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IList<Goal> GetAllClosedGoals()
        {
            var currentUserId = _currentContextProvider.GetCurrentUser().Id;

            return goalRepository.GetList(x => !x.IsOpen &&
                                               (!x.IsPrivate || (x.IsPrivate && x.Users.Any(u => u.Id == currentUserId))),
                x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator,
                x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
                x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams, x => x.Recognitions).
                OrderByDescending(x => x.CreatedDate).ToList();
        }


        public IList<Goal> GetAllByUserId(long id)
        {
            return goalRepository.GetList(x => x.Users.Any(u => u.Id == id) && x.IsOpen && !x.IsPrivate,
               x => x.KeyResults, x => x.GoalStatus,
               x => x.GoalCategory, x => x.EntityCreator, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
               x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams, x => x.Recognitions)
               .OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IList<Goal> PrivateOnlyByUserId(long id)
        {
            var currentUserId = _currentContextProvider.GetCurrentUser().Id;
            return goalRepository.GetList(x => x.Users.Any(u => u.Id == id) && x.IsOpen &&
                                                  (x.IsPrivate && x.Users.Any(u => u.Id == currentUserId)),
                    x => x.KeyResults, x => x.GoalStatus,
                    x => x.GoalCategory, x => x.EntityCreator, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator),
                    x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams, x => x.Recognitions)
                .OrderByDescending(x => x.CreatedDate).ToList();
        }
        public IList<Goal> ByUserName(string userNmae)
        {
            return goalRepository.GetList(x =>
            {
                var firstOrDefault = x.Users.FirstOrDefault();
                return firstOrDefault != null && firstOrDefault.UserName == userNmae && x.IsOpen;
            }, x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Parent, x => x.SubGoals);
        }

        public void AddGoal(Goal goal)
        {
            goal.IsOpen = true;
            Add(goal);

            var activity = new Activity()
            {
                ActorId = _currentContextProvider.GetCurrentUser().Id,
                ActionText = "Created new objective",
                GoalId = goal.Id
            };
            _activityService.Add(activity);
        }
 
        public void RemoveGoals(params Goal[] goals)
        {
            goalRepository.RemoveGoals(goals);
        }

        public void UpdateGoals(params Goal[] goals)
        {
            Remove(goals);
        }

        public void AddGoalToUser(Goal goal, Model.Entities.ApplicationUser[] currentUser)
        {
            goalRepository.AddGoalToUser(goal, currentUser);
        }

        public void AddKeyResultToGoal(Goal goalToUpdate, KeyResult goal)
        {
            goalRepository.AddKeyResultToGoal(goalToUpdate, goal);
        }

        public void UpdateStatus(long goalId, long newStatus)
        {
            goalRepository.UpdateStatus(goalId, newStatus);
        }

        public void UpdatePriority(long goalId, int priority)
        {
            goalRepository.UpdatePriority(goalId, priority);
        }

        public void UpdateCategory(long goalId, long newCategory)
        {
            goalRepository.UpdateCategory(goalId, newCategory);
        }

        public void UpdateTitle(long goalId, string title)
        {
            goalRepository.UpdateTitle(goalId, title);
        }

        public void UpdateProgress(long goalId, long progress)
        {
            goalRepository.UpdateProgress(goalId, progress);
        }


        public void UpdateIsPrivate(long goalId, bool isPrivate)
        {
            goalRepository.UpdateIsPrivate(goalId, isPrivate);
        }

        public void UpdateDueDate(long goalId, DateTime? dueDate)
        {
            goalRepository.UpdateDueDate(goalId, dueDate);
        }

        public void UpdateDes(long goalId, string des)
        {
            goalRepository.UpdateDes(goalId, des);
        }

        public void UpdateParent(long goalId, long? parentId)
        {
            goalRepository.UpdateParent(goalId, parentId);
        }

        public void UpdateIsOpen(long goalId, bool isOpen)
        {
            goalRepository.UpdateIsOpen(goalId, isOpen);
        }


        public void UpdateIndicator(long goalId, long indicatorId, long indicatorValueId)
        {
            var goalIndicatorToUpdate = _goalIndicatorsRepository.GetSingle(x => x.GoalId == goalId && x.IndicatorId == indicatorId);

            var goalIndicator = new GoalIndicator() { GoalId = goalId, IndicatorId = indicatorId, IndicatorValueId = indicatorValueId };
            if (goalIndicatorToUpdate != null)
            {
                _goalIndicatorsRepository.Update(goalIndicator, x => x.IndicatorId);
            }
            else
            {
                _goalIndicatorsRepository.Add(goalIndicator);
            }
        }

        public void AddSubGoal(long parentId, long subgoalId)
        {
            goalRepository.AddSubGoal(parentId, subgoalId);
        }

        public void DeleteSubGoal(long parentId, long subgoalId)
        {
            goalRepository.DeleteSubGoal(parentId, subgoalId);
        }

        public bool IsAligned(long id)
        {
            var existinggoal = GetByGaolId(id);
            return existinggoal.Parent != null || existinggoal.SubGoals.Count > 0;
        }

        public Task<List<Goal>> MostRecognized(int top)
        {
            return goalRepository.MostRecognized(top);
        }

    }
}
