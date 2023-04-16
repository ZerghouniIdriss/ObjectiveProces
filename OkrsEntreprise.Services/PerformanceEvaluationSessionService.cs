using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.Services
{
    public interface IPerformanceEvaluationSessionsService
    {
        void Add(PerformanceEvaluationSession PerformanceEvaluationSession);
        void Update(params PerformanceEvaluationSession[] PerformanceEvaluationSessiones);
        void Remove(params PerformanceEvaluationSession[] PerformanceEvaluationSessiones);

        PerformanceEvaluationSession GetById(long id);
        IList<PerformanceEvaluationSession> GetAll();
        IList<PerformanceEvaluationSession> ByAttendee(long Id);

        Survey GetSurveyByName(string name);

        void CreateSession(PerformanceEvaluationSession PerformanceEvaluationSession, ApplicationUser animator);
    }

    public class PerformanceEvaluationSessionsesService : CRUDServiceBase<PerformanceEvaluationSessionRepository, PerformanceEvaluationSession>, IPerformanceEvaluationSessionsService
    {
        private IPerformanceEvaluationSessionRepository _performanceEvaluationSessionRepository;
        private ISurveyRepository _surveyRepository;
        private IActivityService _activityService;

        public PerformanceEvaluationSessionsesService(IPerformanceEvaluationSessionRepository PerformanceEvaluationSessionRepository, ISurveyRepository surveyRepository, IActivityService activityService)
        {
            this._performanceEvaluationSessionRepository = PerformanceEvaluationSessionRepository;
            this._surveyRepository = surveyRepository;
            this._activityService = activityService;
        }

        public PerformanceEvaluationSession GetById(long id)
        {
            return _performanceEvaluationSessionRepository.GetSingle(x => x.Id == id, x => x.Animator, x => x.Attendee,
                x => x.Goals, x => x.Goals.Select(g => g.KeyResults), x => x.Goals.Select(g => g.Comments), x => x.Goals.Select(g => g.Users),
                x => x.Goals.Select(g => g.SubGoals), x => x.Goals.Select(g => g.GoalStatus), x => x.Goals.Select(g => g.GoalCategory),
                x => x.SessionStatus, x => x.EntityCreator, x => x.PerformanceSurvey, x => x.PerformanceSurvey.Questions, x => x.PerformanceSurvey.Options,
                x => x.PerformanceSurveyUserAnswers);
        }

        public IList<PerformanceEvaluationSession> GetAll()
        {
            return _performanceEvaluationSessionRepository.GetAll(x => x.Animator, x => x.Attendee,
                x => x.Goals, x => x.Goals.Select(g => g.KeyResults), x => x.Goals.Select(g => g.Comments), x => x.Goals.Select(g => g.Users),
                x => x.Goals.Select(g => g.SubGoals), x => x.Goals.Select(g => g.GoalStatus), x => x.Goals.Select(g => g.GoalCategory),
                x => x.SessionStatus, x => x.EntityCreator, x => x.PerformanceSurvey, x => x.PerformanceSurvey.Questions, x => x.PerformanceSurvey.Options,
                x => x.PerformanceSurveyUserAnswers);
        }

        public IList<PerformanceEvaluationSession> ByAttendee(long Id)
        {
            return _performanceEvaluationSessionRepository.GetList(x => x.Attendee.Id == Id, x => x.Animator, x => x.Attendee, x => x.Goals, x => x.SessionStatus, x => x.EntityCreator);
        }

        public Survey GetSurveyByName(string name)
        {
            return _surveyRepository.GetSingle(s => s.Code == name, s => s.Questions, s => s.Options);
        }

        public void CreateSession(PerformanceEvaluationSession performanceEvaluationSession, ApplicationUser animator)
        {
            performanceEvaluationSession.SessionStatus = new SessionStatus() { Id = 1 };
            performanceEvaluationSession.CreatedDate = DateTime.Now;
            performanceEvaluationSession.Animator = animator.Id == performanceEvaluationSession.Attendee.Id ? performanceEvaluationSession.Attendee : new ApplicationUser() { Id = animator.Id };

            foreach (var item in performanceEvaluationSession.PerformanceSurveyUserAnswers)
            {
                item.SurveyId = performanceEvaluationSession.PerformanceSurveyId;
                item.UserId = performanceEvaluationSession.Attendee.Id;
            }

            this._performanceEvaluationSessionRepository.CreateSession(performanceEvaluationSession); 
        }
    }
}
