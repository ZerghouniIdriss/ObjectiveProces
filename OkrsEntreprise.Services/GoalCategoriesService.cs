using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Services
{
    public interface IGoalCategoriesService
    {
        void Add(GoalCategory goalCategory);
        void Update(params GoalCategory[] goalCategories);
        void Remove(params GoalCategory[] goalCategories);
        GoalCategory GetByGoalCategoryId(long id);
        IList<GoalCategory> GetAll();
    }

    class GoalCategoriesService : CRUDServiceBase<GoalCategoryRepository, GoalCategory>, IGoalCategoriesService
    {
        private IGoalCategoryRepository goalCategoryRepository;
        private IActivityService _activityService;

        public GoalCategoriesService(IGoalCategoryRepository goalCategoryRepository, IActivityService activityService)
        {
            this.goalCategoryRepository = goalCategoryRepository;
            this._activityService = activityService;
        }

        public new void Add(GoalCategory goalCategory)
        {
            base.Add(goalCategory); 
        }

        public new void Update(params GoalCategory[] goalCategories)
        {
            base.Update(goalCategories); 
        }

        public new void Remove(params GoalCategory[] goalCategories)
        {
            base.Remove(goalCategories); 
        }


        public IList<GoalCategory> GetAll()
        {
            return this.goalCategoryRepository.GetAll();
        }

        public GoalCategory GetByGoalCategoryId(long id)
        {
            return this.goalCategoryRepository.GetSingle(x => x.Id == id);
        }
    }
}
