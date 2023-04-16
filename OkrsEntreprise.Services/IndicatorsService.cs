using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface IIndicatorsService:IServiceBase<Indicator>
    {
    }
    public class IndicatorsService : CRUDServiceBase<IndicatorRepository, Indicator>, IIndicatorsService
    {
        public override IList<Indicator> GetAll()
        {
            return Repository.GetAllWithIndicators();
        }
    }
}
