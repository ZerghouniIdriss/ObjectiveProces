using OkrsEntreprise.Model.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface ISurveyRepository : IRepositoryBase<Survey>
    {
    }

    public class SurveyRepository : RepositoryBase<Survey>, ISurveyRepository
    {
    }
}
