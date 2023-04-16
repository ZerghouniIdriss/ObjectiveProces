//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OkrsEntreprise.DAL.Context;
//using OkrsEntreprise.DAL.Repositories;
//using OkrsEntreprise.Model.Entities;

//namespace OkrsEntreprise.Services
//{
//    public interface IActivityVerbService
//    {

//        void Add(ActivityVerb ActivityVerb);
//        void Update(params ActivityVerb[] departments);
//        void Remove(params ActivityVerb[] departments);


//        ActivityVerb GetById(long id);
//        IList<ActivityVerb> GetAll(); 
//    }

//    public class ActivityVerbService : CRUDServiceBase<ActivityVerbRepository, ActivityVerb>, IActivityVerbService
//    {
//        private IActivityVerbRepository ActivityVerbRepository;

//        public ActivityVerbService(IActivityVerbRepository ActivityVerbRepository)
//        {
//           this.ActivityVerbRepository= ActivityVerbRepository;
//        }

//        public ActivityVerb GetById(long id)
//        {
//            return ActivityVerbRepository.GetSingle(x => x.Id == id);
//        }

//        public IList<ActivityVerb> GetAll()
//        {
//            return ActivityVerbRepository.GetAll();
//        }

       
//    }
//}
