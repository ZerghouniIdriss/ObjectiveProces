using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IOneOnOneSessionRepository : IRepositoryBase<OneOnOneSession>
    {
        void CreateSession(OneOnOneSession oneOnOneSession);
    }

    public class OneOnOneSessionRepository : RepositoryBase<OneOnOneSession>, IOneOnOneSessionRepository
    {
        public void CreateSession(OneOnOneSession oneOnOneSession)
        {
            using (var context = new OkrsContext())
            {
                if (oneOnOneSession.Animator != null)
                {
                    context.Users.Attach(oneOnOneSession.Animator);
                }

                if (oneOnOneSession.Attendee != null && oneOnOneSession.Attendee.Id != oneOnOneSession.Animator.Id)
                {
                    context.Users.Attach(oneOnOneSession.Attendee);
                }

                if (oneOnOneSession.SessionStatus != null)
                {
                    context.SessionStatus.Attach(oneOnOneSession.SessionStatus);
                }

                foreach (var goal in oneOnOneSession.Goals)
                {
                    goal.Comments = null;
                    goal.KeyResults = null;

                    context.Goals.Add(goal);
                    context.Goals.Attach(goal);
                }

                context.OneOnOneSessions.Add(oneOnOneSession);
                //context.Goals.Attach(gObj);

                SaveContextChange(context);
            }

        }
    }
}