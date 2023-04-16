using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IPerformanceEvaluationSessionRepository : IRepositoryBase<PerformanceEvaluationSession>
    {
        void CreateSession(PerformanceEvaluationSession PerformanceEvaluationSession);
    }

    public class PerformanceEvaluationSessionRepository : RepositoryBase<PerformanceEvaluationSession>, IPerformanceEvaluationSessionRepository
    {
        public void CreateSession(PerformanceEvaluationSession PerformanceEvaluationSession)
        {
            using (var context = new OkrsContext())
            {
                if (PerformanceEvaluationSession.Animator != null)
                {
                    context.Users.Attach(PerformanceEvaluationSession.Animator);
                }

                if (PerformanceEvaluationSession.Attendee != null && PerformanceEvaluationSession.Attendee.Id != PerformanceEvaluationSession.Animator.Id)
                {
                    context.Users.Attach(PerformanceEvaluationSession.Attendee);
                }

                if (PerformanceEvaluationSession.SessionStatus != null)
                {
                    context.SessionStatus.Attach(PerformanceEvaluationSession.SessionStatus);
                }

                foreach (var goal in PerformanceEvaluationSession.Goals)
                {
                    goal.Comments = null;
                    goal.KeyResults = null;

                    context.Goals.Add(goal);
                    context.Goals.Attach(goal);
                }

                context.PerformanceEvaluationSessions.Add(PerformanceEvaluationSession);
                //context.Goals.Attach(gObj);

                SaveContextChange(context);
            }

        }
    }
}