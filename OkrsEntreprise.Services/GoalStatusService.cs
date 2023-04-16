using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Services
{
    public interface IGoalStatusService
    {
        void Add(GoalStatus goalStatus);
        void Update(params GoalStatus[] goalStatus);
        void Remove(params GoalStatus[] goalStatus);
        GoalStatus GetByGoalStatusId(long id);
        IList<GoalStatus> GetAll();
    }

    public class GoalStatusService : CRUDServiceBase<GoalStatusRepository, GoalStatus>, IGoalStatusService
    {
        private IGoalStatusRepository _goalStatusRepository; 

        public GoalStatusService(IGoalStatusRepository goalStatusRepository )
        {
            this._goalStatusRepository = goalStatusRepository; 
        }

        public new void Add(GoalStatus goalStatus)
        {
            base.Add(goalStatus); 
        }

        public new void Update(params GoalStatus[] goalStatus)
        {
            base.Update(goalStatus); 
        }

        public new void Remove(params GoalStatus[] goalStatus)
        {
            base.Remove(goalStatus); 
        }

        public IList<GoalStatus> GetAll()
        {
            return this._goalStatusRepository.GetAll();
        }

        public GoalStatus GetByGoalStatusId(long id)
        {
            return this._goalStatusRepository.GetSingle(x => x.Id == id);
        }
    }
}
