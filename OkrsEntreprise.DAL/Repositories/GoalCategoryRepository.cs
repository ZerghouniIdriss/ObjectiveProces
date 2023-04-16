using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IGoalCategoryRepository : IRepositoryBase<GoalCategory>
    {

    }

    public class GoalCategoryRepository : RepositoryBase<GoalCategory>, IGoalCategoryRepository
    {
    }
}
