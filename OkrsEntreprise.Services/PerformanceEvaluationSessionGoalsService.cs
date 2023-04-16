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
    public interface IPerformanceEvaluationSessionGolasService
    {
        IList<Goal> GetPreviousGoalsByPerformanceEvaluationSessionId(long Id);
        void CreatGoalInOnPerformanceEvaluationSession(Goal goal, long PerformanceEvaluationSessionId);
        IList<Goal> GetAllPerformanceEvaluationSessionGoalsBySessionId(long PerformanceEvaluationSessionId);
        IList<Goal> GetAllPerformanceEvaluationSessionPreviousGoalsByUserId(long userId);
        IList<Goal> GetAllPerformanceEvaluationSessionNextGoalsByUserId(long userId);
    }

    public class PerformanceEvaluationSessionGolasService : CRUDServiceBase<PerformanceEvaluationSessionRepository, PerformanceEvaluationSession>, IPerformanceEvaluationSessionGolasService
    {
        private IPerformanceEvaluationSessionRepository _PerformanceEvaluationSessionRepository;
        private IGoalRepository _goalRepository;
        private IActivityService _activityService;

        public PerformanceEvaluationSessionGolasService(IPerformanceEvaluationSessionRepository PerformanceEvaluationSessionRepository, IGoalRepository goalRepository, IActivityService activityService)
        {
            _PerformanceEvaluationSessionRepository = PerformanceEvaluationSessionRepository;
            _goalRepository = goalRepository;
            _activityService = activityService;
        }

        public IList<Goal> GetAllPerformanceEvaluationSessionGoalsBySessionId(long PerformanceEvaluationSessionId)
        {
            var PerformanceEvaluationSession = _PerformanceEvaluationSessionRepository.GetSingle(x => x.Id == PerformanceEvaluationSessionId,
                x => x.EntityCreator, x => x.Goals, x => x.SessionStatus,x=>x.Animator, x => x.Attendee);

            return PerformanceEvaluationSession.Goals.ToList();
        }
         
        public IList<Goal> GetPreviousGoalsByPerformanceEvaluationSessionId(long PerformanceEvaluationSessionId)
        {
           var PerformanceEvaluationSession = _PerformanceEvaluationSessionRepository.GetSingle(x => x.Id == PerformanceEvaluationSessionId,
                 x => x.EntityCreator, x => x.Goals, x => x.SessionStatus, x => x.Animator, x => x.Attendee);

           return PerformanceEvaluationSession.Goals.Where(goalItem => (goalItem.GoalStatusId != 1) && (goalItem.GoalCategory.Id != 3)).ToList();
        } 

        public void CreatGoalInOnPerformanceEvaluationSession(Goal goal, long PerformanceEvaluationSessionId )
        {
            var PerformanceEvaluationSession = _PerformanceEvaluationSessionRepository.GetSingle(x => x.Id == PerformanceEvaluationSessionId, x => x.Goals);
            PerformanceEvaluationSession.Goals.Add(goal);
            _PerformanceEvaluationSessionRepository.Update(PerformanceEvaluationSession,x=>x.Goals);
         
        }

        public IList<Goal> GetAllPerformanceEvaluationSessionPreviousGoalsByUserId(long userId)
        {
            // all ther goals by user with category== 1-on-1 Session and status not reviewd yet 
            return _goalRepository.GetList(x => x.Users.Where(u => u.Id == userId).Count() > 0 && x.GoalCategory.Id == 3 && x.GoalStatusId != 3,                    
                    x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Parent,
                    x => x.SubGoals, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator), x => x.PerformanceEvaluationSessions,x => x.Users);
             
        }

        public IList<Goal> GetAllPerformanceEvaluationSessionNextGoalsByUserId(long userId)
        {
            // all ther goals by user with category== 1-on-1 Session and status not reviewd yet 
            return _goalRepository.GetList(x => x.Users.Where(u => u.Id == userId).Count() > 0 && x.GoalCategory.Id == 3 && x.GoalStatusId != 3,
                    x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Parent,
                    x => x.SubGoals, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator), x => x.PerformanceEvaluationSessions, x => x.Users);
        }
    }
}
