using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IIndicatorRepository : IRepositoryBase<Indicator>
    {
        IList<Indicator> GetAllWithIndicators();
    }

    public class IndicatorRepository : RepositoryBase<Indicator>, IIndicatorRepository
    {
        public  IList<Indicator> GetAllWithIndicators()
        {
            return base.GetAll(x=>x.IndicatorValues);
        }
    }
}
