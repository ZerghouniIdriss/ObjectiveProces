using AutoMapper;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Model.Entities.Surveys;
using OkrsEntreprise.Api.App_Start;

namespace OkrsEntreprise.Api.Controllers
{

    [RoutePrefix("api/PerformanceEvaluationSession")]
    public class PerformanceEvaluationSessionController : BaseController
    {
        private IPerformanceEvaluationSessionsService _performanceEvaluationSessionsService;
        private IPerformanceEvaluationSessionGolasService _onOneSessionGolasService;
        public PerformanceEvaluationSessionController(IUserService userService, IPerformanceEvaluationSessionsService PerformanceEvaluationSessionsService, IPerformanceEvaluationSessionGolasService onOneSessionGolasService) : base(userService)
        {
            _performanceEvaluationSessionsService = PerformanceEvaluationSessionsService;
            _onOneSessionGolasService = onOneSessionGolasService;
        }

        // GET: api/PerformanceEvaluationSession 
        public IHttpActionResult Get()
        {
            var PerformanceEvaluationSessions = _performanceEvaluationSessionsService.GetAll();

            List<PerformanceEvaluationSessionViewModel> PerformanceEvaluationSessionsViewModel = new List<PerformanceEvaluationSessionViewModel>();

            foreach (var item in PerformanceEvaluationSessions)
            {
                PerformanceEvaluationSessionsViewModel.Add(Mapper.Map<PerformanceEvaluationSessionViewModel>(item));
            }

            return Ok(PerformanceEvaluationSessionsViewModel.ToArray());
        }

        // GET: api/PerformanceEvaluationSession/5
        public HttpResponseMessage Get(int id)
        {
            var PerformanceEvaluationSessions = _performanceEvaluationSessionsService.GetById(id);

            var PerformanceEvaluationSessionsViewModel = Mapper.Map<PerformanceEvaluationSessionViewModel>(PerformanceEvaluationSessions);

            return Request.CreateResponse(HttpStatusCode.OK, PerformanceEvaluationSessionsViewModel);
        }

        // POST: api/PerformanceEvaluationSession
        public HttpResponseMessage Post([FromBody]PerformanceEvaluationSessionViewModel PerformanceEvaluationSessionViewModel)
        {
            var PerformanceEvaluationSession = Mapper.Map<PerformanceEvaluationSession>(PerformanceEvaluationSessionViewModel);

            _performanceEvaluationSessionsService.Add(PerformanceEvaluationSession);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/PerformanceEvaluationSession/5
        public HttpResponseMessage Put(int id, PerformanceEvaluationSessionViewModel PerformanceEvaluationSessionViewModel)
        {
            var PerformanceEvaluationSessionToUpdate = _performanceEvaluationSessionsService.GetById(id);

            _performanceEvaluationSessionsService.Update(PerformanceEvaluationSessionToUpdate);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //// DELETE: api/PerformanceEvaluationSession/5
        //public HttpResponseMessage Delete(int id)
        //{
        //    var PerformanceEvaluationSessionToDelete = _performanceEvaluationSessionsService.GetById(id);

        //    _performanceEvaluationSessionsService.Remove(PerformanceEvaluationSessionToDelete);

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}


        [HttpPost]
        [Route("PreviousGoals")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PreviousGoals(AssignToViewModel assignToViewModel)
        {
            if (assignToViewModel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            var previousGoals = _onOneSessionGolasService.GetAllPerformanceEvaluationSessionPreviousGoalsByUserId(assignToViewModel.id);

            var goalModels = new List<SessionGoalViewModel>();

            foreach (var item in previousGoals)
            {
                goalModels.Add(Mapper.Map<SessionGoalViewModel>(item));
            }
            return Ok(goalModels.ToArray());
        }

        [HttpPost]
        [Route("NextGoals")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult NextGoals(AssignToViewModel assignToViewModel)
        {
            if (assignToViewModel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            var previousGoals = _onOneSessionGolasService.GetAllPerformanceEvaluationSessionNextGoalsByUserId(assignToViewModel.id);

            var goalModels = new List<SessionGoalViewModel>();

            foreach (var item in previousGoals)
            {
                goalModels.Add(Mapper.Map<SessionGoalViewModel>(item));
            }
            return Ok(goalModels.ToArray());
        }

        [HttpPost]
        [Route("SearchGoals")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult SearchGoals(AssignToViewModel assignToViewModel)
        {
            if (assignToViewModel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            var previousGoals = _onOneSessionGolasService.GetAllPerformanceEvaluationSessionPreviousGoalsByUserId(assignToViewModel.id);

            var goalModels = new List<SessionGoalViewModel>();

            foreach (var item in previousGoals)
            {
                goalModels.Add(Mapper.Map<SessionGoalViewModel>(item));
            }
            return Ok(goalModels.ToArray());
        }

        [HttpPost]
        [Route("CreateSession")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult CreateSession([FromBody]PerformanceEvaluationSessionViewModel PerformanceEvaluationSessionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var PerformanceEvaluationSession = Mapper.Map<PerformanceEvaluationSession>(PerformanceEvaluationSessionViewModel);

            try
            {
                _performanceEvaluationSessionsService.CreateSession(PerformanceEvaluationSession, CurrentUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        [HttpPost]
        [Route("GetSession")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSession([FromBody]long id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Invalid Request");
                return BadRequest(ModelState);
            }

            PerformanceEvaluationSession session = null;
            try
            {
                session = _performanceEvaluationSessionsService.GetById(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            PerformanceEvaluationSessionViewModel performanceEvaluationSessionViewModel = Mapper.Map<PerformanceEvaluationSessionViewModel>(session);
            performanceEvaluationSessionViewModel.Animator.Avatar = CommonHelperMethods.ResolveAvatarPath(performanceEvaluationSessionViewModel.Animator.Avatar);
            performanceEvaluationSessionViewModel.Attendee.Avatar = CommonHelperMethods.ResolveAvatarPath(performanceEvaluationSessionViewModel.Attendee.Avatar);

            return Ok(performanceEvaluationSessionViewModel);
        }


        [HttpPost]
        [Route("GetSessions")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSessions()
        {
            IList<PerformanceEvaluationSession> PerformanceEvaluationSessions = null;

            try
            {
                PerformanceEvaluationSessions = _performanceEvaluationSessionsService.GetAll();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ex.Message);
            }

            List<ListPerformanceEvaluationSessionViewModel> PerformanceEvaluationSessionsViewModel = new List<ListPerformanceEvaluationSessionViewModel>();

            foreach (var item in PerformanceEvaluationSessions)
            {
                ListPerformanceEvaluationSessionViewModel listPerformanceEvaluationSessionViewModel = Mapper.Map<ListPerformanceEvaluationSessionViewModel>(item);
                listPerformanceEvaluationSessionViewModel.Animator.Avatar = CommonHelperMethods.ResolveAvatarPath(listPerformanceEvaluationSessionViewModel.Animator.Avatar);
                listPerformanceEvaluationSessionViewModel.Attendee.Avatar = CommonHelperMethods.ResolveAvatarPath(listPerformanceEvaluationSessionViewModel.Attendee.Avatar);

                PerformanceEvaluationSessionsViewModel.Add(listPerformanceEvaluationSessionViewModel);
            }

            return Ok(PerformanceEvaluationSessionsViewModel.ToArray());

        }


        [HttpPost]
        [Route("GetSurvey")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSurvey(SurveyViewModel surveyViewModel)
        {
            if (string.IsNullOrEmpty(surveyViewModel.code))
            {
                ModelState.AddModelError("", "Invalid Request");
                return BadRequest(ModelState);
            }

            Survey survey = null;

            try
            {
                survey = _performanceEvaluationSessionsService.GetSurveyByName(surveyViewModel.code);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(Mapper.Map<SurveyViewModel>(survey));
        }


        [HttpPost]
        [Route("Delete")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete([FromBody]long id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Invalid Request");
                return BadRequest(ModelState);
            }


            try
            {
                PerformanceEvaluationSession perEvalSession = new PerformanceEvaluationSession() { Id = id };
                _performanceEvaluationSessionsService.Remove(perEvalSession);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(true);
        }

    }
}


