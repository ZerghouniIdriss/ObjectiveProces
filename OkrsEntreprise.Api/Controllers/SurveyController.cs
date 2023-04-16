using AutoMapper;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Survey")]
    public class SurveyController : BaseController
    {
        private ISurveysService _surveysService = null;

        public SurveyController(IUserService usersService, ISurveysService surveysService) : base(usersService)
        {
            this._surveysService = surveysService;
        }
        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(SurveyViewModel surveyViewModel)
        {
           try
            {
                var surveys = _surveysService.GetAll();
                var surveyViewModels = new List<SurveyViewModel>();

                foreach (var item in surveys)
                {
                    surveyViewModels.Add(Mapper.Map<SurveyViewModel>(item));
                }

                return Ok(surveyViewModels.ToArray());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create([FromBody]SurveyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            { 
                Survey survey = new Survey()
                {
                    Code  = viewmodel.code,
                    Description = viewmodel.description
                };
                //survey.EntityCreator = CurrentUser;
                _surveysService.AddSurvey(survey);

                return Ok(Mapper.Map<SurveyViewModel>(survey));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("EditSurvey")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditSurvey([FromBody]SurveyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Survey survey = new Survey() { Id = viewmodel.id  };
                _surveysService.UpdateSurvey(survey);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("DeleteSurvey")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSurvey([FromBody]SurveyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Survey survey = new Survey() { Id = viewmodel.id };
               _surveysService.RemoveSurvey(survey);
                return Ok();
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}
