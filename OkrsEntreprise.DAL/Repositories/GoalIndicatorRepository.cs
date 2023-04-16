using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IGoalIndicatorRepository : IRepositoryBase<GoalIndicator>
    {

    }

    public class GoalIndicatorRepository : RepositoryBase<GoalIndicator>, IGoalIndicatorRepository
    {
    }
}
