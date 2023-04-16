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
    public interface IOneOnOneSessionsService
    {
        void Add(OneOnOneSession oneOnOneSession);
        void Update(params OneOnOneSession[] OneOnOneSessiones);
        void Remove(params OneOnOneSession[] OneOnOneSessiones);

        OneOnOneSession GetById(long id);
        IList<OneOnOneSession> GetAll();
        IList<OneOnOneSession> ByAttendee(long Id);

        void CreateSession(OneOnOneSession oneOnOneSession, ApplicationUser animator);
    }

    public class OneOnOneSessionsesService : CRUDServiceBase<OneOnOneSessionRepository, OneOnOneSession>, IOneOnOneSessionsService
    {
        private IOneOnOneSessionRepository OneOnOneSessionRepository;
        private IActivityService _acitivityService;
        public OneOnOneSessionsesService(IOneOnOneSessionRepository OneOnOneSessionRepository, IActivityService activityService)
        {
            this.OneOnOneSessionRepository = OneOnOneSessionRepository;
            this._acitivityService = activityService;
        }

        public OneOnOneSession GetById(long id)
        {
            return OneOnOneSessionRepository.GetSingle(x => x.Id == id, x => x.Animator, x => x.Attendee,
                x => x.Goals, x => x.Goals.Select(g => g.KeyResults), x => x.Goals.Select(g => g.Comments), x => x.Goals.Select(g => g.Users),
                x => x.Goals.Select(g => g.SubGoals), x => x.Goals.Select(g => g.GoalStatus), x => x.Goals.Select(g => g.GoalCategory),
                x => x.SessionStatus, x => x.EntityCreator);
        }

        public IList<OneOnOneSession> GetAll()
        {
            return OneOnOneSessionRepository.GetAll(x => x.Animator, x => x.Attendee,
                x => x.Goals, x => x.Goals.Select(g => g.KeyResults), x => x.Goals.Select(g => g.Comments), x => x.Goals.Select(g => g.Users),
                x => x.Goals.Select(g => g.SubGoals), x => x.Goals.Select(g => g.GoalStatus), x => x.Goals.Select(g => g.GoalCategory),
                x => x.SessionStatus, x => x.EntityCreator);
        }

        public IList<OneOnOneSession> ByAttendee(long Id)
        {
            return OneOnOneSessionRepository.GetList(x => x.Attendee.Id == Id, x => x.Animator, x => x.Attendee, x => x.Goals, x => x.SessionStatus, x => x.EntityCreator);
        }

        public void CreateSession(OneOnOneSession oneOnOneSession, ApplicationUser animator)
        {
            oneOnOneSession.SessionStatus = new SessionStatus() { Id = 1 };
            oneOnOneSession.CreatedDate = DateTime.Now;
            oneOnOneSession.Animator = animator.Id == oneOnOneSession.Attendee.Id ? oneOnOneSession.Attendee : new ApplicationUser() { Id = animator.Id };

            this.OneOnOneSessionRepository.CreateSession(oneOnOneSession);
             
        }
    }
}
