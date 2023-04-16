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
    public interface IKeyResultsService
    {

        void Add(KeyResult KeyResult);

        void Add(KeyResult[] KeyResults);

        void Update(params KeyResult[] departments);
        void Remove(params KeyResult[] departments);


        KeyResult GetById(long id);
        IList<KeyResult> GetAll();
        IEnumerable<KeyResult> ByGoalId(long id);

        void AddKeyResultToGoal(Goal goalToUpdate, KeyResult keyResult);
        KeyResult EditKeyResult(KeyResult keyResult);

        KeyResult DeleteKeyResult(KeyResult keyResultToDelete);

    }

    public class KeyResultsService : CRUDServiceBase<KeyResultRepository, KeyResult>, IKeyResultsService
    {

        private IKeyResultRepository keyResultRepository;
        private IActivityService _activityService;

        public KeyResultsService(IKeyResultRepository keyResultRepository, IActivityService activityService)
        {
            this.keyResultRepository = keyResultRepository;
            _activityService = activityService;
        }

        void IKeyResultsService.Add(KeyResult[] KeyResults)
        {
            keyResultRepository.Add(KeyResults);
        }

        public KeyResult GetById(long id)
        {
            return keyResultRepository.GetSingle(x => x.Id == id);
        }

        public IList<KeyResult> GetAll()
        {
            return keyResultRepository.GetAll(x => x.Goal);
        }


        public IEnumerable<KeyResult> ByGoalId(long Id)
        {
            return keyResultRepository.GetAll(x => x.Goal.Id == Id);
        }

        public void AddKeyResultToGoal(Goal goalToUpdate, KeyResult keyResult)
        {
            this.keyResultRepository.AddKeyResultToGoal(goalToUpdate, keyResult);
            //_activityService.Add("Assigned", goalToUpdate.ToString(), keyResult.Title);
        }

        public KeyResult EditKeyResult(KeyResult keyResult)
        {
            var result = this.keyResultRepository.EditKeyResult(keyResult);
            //_activityService.Add("Updated", keyResult.ToString());
            return result;
        }

        public KeyResult DeleteKeyResult(KeyResult keyResultToDelete)
        {
            var result = this.keyResultRepository.DeleteKeyResult(keyResultToDelete);
            //_activityService.Add("Deleted", keyResultToDelete.ToString());
            return result;
        }

    }
}
