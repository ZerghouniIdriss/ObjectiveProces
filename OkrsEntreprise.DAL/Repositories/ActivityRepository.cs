using System.Collections.Generic;
using OkrsEntreprise.Model.Entities;
using System.Linq;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IActivityRepository : IRepositoryBase<Activity>
    {
        IList<Activity> GetAllWithActorAndGoal();
    }

    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public IList<Activity> GetAllWithActorAndGoal()
        {
           return GetAll(x => x.Actor, x => x.Goal);
        }
    }
}
