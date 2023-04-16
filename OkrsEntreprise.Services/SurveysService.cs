using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.Services
{
    public interface ISurveysService
    {

        void AddSurvey(Survey Survey);
        void UpdateSurvey(params Survey[] departments);
        void RemoveSurvey(params Survey[] departments);


        Survey GetById(long id);
        IList<Survey> GetAll();
        IList<Survey> GetList(Func<Survey, bool> where, params Expression<Func<Survey, object>>[] navigationProperties);
    }

    public class SurveysService : CRUDServiceBase<SurveyRepository, Survey>, ISurveysService
    {
        private ISurveyRepository SurveyRepository;
        private IActivityService _activityService;
        public SurveysService(ISurveyRepository SurveyRepository, IActivityService activityService)
        {
           this.SurveyRepository= SurveyRepository;
           this._activityService = activityService;
        }

        public Survey GetById(long id)
        {
            return SurveyRepository.GetSingle(x => x.Id == id);
        }

        public IList<Survey> GetAll()
        {
            return SurveyRepository.GetAll(x => x.Options, x => x.Questions);
        }

        public IList<Survey> GetList( Func<Survey, bool> where, params Expression<Func<Survey, object>>[] navigationProperties){
            return SurveyRepository.GetList(where, navigationProperties);
        }


        public void AddSurvey(Survey Survey)
        {
            SurveyRepository.Add(Survey); 
        }

        public void UpdateSurvey(params Survey[] departments)
        {
            SurveyRepository.Update(departments); 
        }
        public void RemoveSurvey(params Survey[] departments)
        {
            SurveyRepository.Remove(departments); 
        }

    }
}
