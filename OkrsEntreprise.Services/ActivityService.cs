using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Framework;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface IActivityService
    {
        void Add(Activity activity);
        void Update(params Activity[] departments);
        void Remove(params Activity[] departments);

        Activity GetById(long id); 
        IList<Activity> GetAll(int take);
        IList<Activity> GetAll();
    }

    public class ActivityService : CRUDServiceBase<ActivityRepository, Activity>, IActivityService
    {
        private IActivityRepository _activityRepository;
        private ICurrentContextProvider<ApplicationUser> _contextProvider; 
        public ActivityService(IActivityRepository activityRepository, ICurrentContextProvider<ApplicationUser> currentContextProvider)
        {
           this._contextProvider = currentContextProvider;
           this._activityRepository= activityRepository;
        } 

        public IList<Activity> GetAll(int take)
        {
            return _activityRepository.GetAll(take,x => x.Actor,x => x.Goal);
        }

        public IList<Activity> GetAll()
        {
            return _activityRepository.GetAllWithActorAndGoal();
        }


        //public void Add(string verbCode, string objectAffected, string[] targets)
        //{
        //    List<Activity> activities = new List<Activity>();
        //    var currentUser = _contextProvider.GetCurrentUser();
        //    foreach (string target in targets)
        //    {
        //        Activity activity = new Activity();
        //        activity.ActivityObject = objectAffected;
        //        activity.ActivityActorId = currentUser.Id;
        //        activity.ActivityTarget = target;
        //        activity.ActivityVerb = new ActivityVerb { Text = verbCode };
        //        activity.CreatedDate = DateTime.Now;
        //        activities.Add(activity);
        //    }
        //    _activityRepository.Add(activities.ToArray());
        //}



        //public void Add(string verbCode, string objectAffected, string target=null)
        //{          
        //    //// 1- you get the verb text by code
        //    //// 2- Get currentUserusing _contextProvider
        //    //// 3- Check if target is not null then add them otherwise ignor
        //    //// 4-call your repository to persist the activity 
        //    //var currentUser = _contextProvider.GetCurrentUser();            
        //    //Activity activity = new Activity();            
        //    //activity.ActivityActorId = currentUser.Id;
        //    //activity.ActivityObject = objectAffected;
        //    //activity.ActivityTarget = string.IsNullOrEmpty(target) ? string.Empty : target;
        //    //activity.ActivityVerb = new ActivityVerb { Text = verbCode };
        //    //activity.CreatedDate = DateTime.Now;
        //    //_activityRepository.Add(activity);            
        //} 
    }
}
