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
    public interface IOneOnOneSessionGolasService
    {
        IList<Goal> GetPreviousGoalsByOneOnOneSessionId(long Id);
        void CreatGoalInOnOneOnOneSession(Goal goal, long oneOnOneSessionId);
        IList<Goal> GetAllOneOnOneSessionGoalsBySessionId(long oneOnOneSessionId);
        IList<Goal> GetAllOneOnOneSessionPreviousGoalsByUserId(long userId);
        IList<Goal> GetAllOneOnOneSessionNextGoalsByUserId(long userId);
    }

    public class OneOnOneSessionGolasService : CRUDServiceBase<OneOnOneSessionRepository, OneOnOneSession>, IOneOnOneSessionGolasService
    {
        private IOneOnOneSessionRepository _oneOnOneSessionRepository;
        private IGoalRepository _goalRepository;
        private IActivityService _activityService;

        public OneOnOneSessionGolasService(IOneOnOneSessionRepository oneOnOneSessionRepository, IGoalRepository goalRepository, IActivityService activityService)
        {
            _oneOnOneSessionRepository = oneOnOneSessionRepository;
            _goalRepository = goalRepository;
            _activityService = activityService;
        }

        public IList<Goal> GetAllOneOnOneSessionGoalsBySessionId(long oneOnOneSessionId)
        {
            var oneOnOneSession = _oneOnOneSessionRepository.GetSingle(x => x.Id == oneOnOneSessionId,
                x => x.EntityCreator, x => x.Goals, x => x.SessionStatus,x=>x.Animator, x => x.Attendee);

            return oneOnOneSession.Goals.ToList();
        }
         
        public IList<Goal> GetPreviousGoalsByOneOnOneSessionId(long oneOnOneSessionId)
        {
           var oneOnOneSession = _oneOnOneSessionRepository.GetSingle(x => x.Id == oneOnOneSessionId,
                 x => x.EntityCreator, x => x.Goals, x => x.SessionStatus, x => x.Animator, x => x.Attendee);

           return oneOnOneSession.Goals.Where(goalItem => (goalItem.GoalStatusId != 1) && (goalItem.GoalCategory.Id != 3)).ToList();
        } 

        public void CreatGoalInOnOneOnOneSession(Goal goal, long oneOnOneSessionId )
        {
            var oneOnOneSession = _oneOnOneSessionRepository.GetSingle(x => x.Id == oneOnOneSessionId, x => x.Goals);
            oneOnOneSession.Goals.Add(goal);
            _oneOnOneSessionRepository.Update(oneOnOneSession,x=>x.Goals);
    
        }

        public IList<Goal> GetAllOneOnOneSessionPreviousGoalsByUserId(long userId)
        {
            // all ther goals by user with category== 1-on-1 Session and status not reviewd yet 
            return _goalRepository.GetList(x => x.Users.Where(u => u.Id == userId).Count() > 0 && x.GoalCategory.Id == 3 && x.GoalStatusId != 3,                    
                    x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Parent,
                    x => x.SubGoals, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator), x => x.OneOnOneSessions,x => x.Users);
             
        }

        public IList<Goal> GetAllOneOnOneSessionNextGoalsByUserId(long userId)
        {
            // all ther goals by user with category== 1-on-1 Session and status not reviewd yet 
            return _goalRepository.GetList(x => x.Users.Where(u => u.Id == userId).Count() > 0 && x.GoalCategory.Id == 3 && x.GoalStatusId != 3,
                    x => x.KeyResults, x => x.GoalStatus, x => x.GoalCategory, x => x.EntityCreator, x => x.Parent,
                    x => x.SubGoals, x => x.Comments, x => x.Comments.Select(y => y.EntityCreator), x => x.OneOnOneSessions, x => x.Users);
        }
    }
}
