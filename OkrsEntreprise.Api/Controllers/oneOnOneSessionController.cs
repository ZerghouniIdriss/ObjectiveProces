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
using OkrsEntreprise.Api.App_Start;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/OneOnOneSession")]
    public class OneOnOneSessionController : BaseController
    {
        private IOneOnOneSessionsService _oneOnOneSessionsService;
        private IOneOnOneSessionGolasService _onOneSessionGolasService;
        public OneOnOneSessionController(IUserService usersService, IOneOnOneSessionsService oneOnOneSessionsService, IOneOnOneSessionGolasService onOneSessionGolasService) : base(usersService)
        {
            _oneOnOneSessionsService = oneOnOneSessionsService;
            _onOneSessionGolasService = onOneSessionGolasService;
        }

        // GET: api/oneOnOneSession 
        public IHttpActionResult Get()
        {
            var oneOnOneSessions = _oneOnOneSessionsService.GetAll();

            List<OneOnOneSessionViewModel> oneOnOneSessionsViewModel = new List<OneOnOneSessionViewModel>();

            foreach (var item in oneOnOneSessions)
            {
                oneOnOneSessionsViewModel.Add(Mapper.Map<OneOnOneSessionViewModel>(item));
            }

            return Ok(oneOnOneSessionsViewModel.ToArray());
        }

        // GET: api/oneOnOneSession/5
        public HttpResponseMessage Get(int id)
        {
            var oneOnOneSessions = _oneOnOneSessionsService.GetById(id);

            var oneOnOneSessionsViewModel = Mapper.Map<OneOnOneSessionViewModel>(oneOnOneSessions);

            return Request.CreateResponse(HttpStatusCode.OK, oneOnOneSessionsViewModel);
        }

        // POST: api/oneOnOneSession
        public HttpResponseMessage Post([FromBody]OneOnOneSessionViewModel oneOnOneSessionViewModel)
        {
            var oneOnOneSession = Mapper.Map<OneOnOneSession>(oneOnOneSessionViewModel);

            _oneOnOneSessionsService.Add(oneOnOneSession);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/oneOnOneSession/5
        public HttpResponseMessage Put(int id, OneOnOneSessionViewModel oneOnOneSessionViewModel)
        {
            var oneOnOneSessionToUpdate = _oneOnOneSessionsService.GetById(id);

            _oneOnOneSessionsService.Update(oneOnOneSessionToUpdate);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/oneOnOneSession/5
        public HttpResponseMessage Delete(int id)
        {
            var oneOnOneSessionToDelete = _oneOnOneSessionsService.GetById(id);

            _oneOnOneSessionsService.Remove(oneOnOneSessionToDelete);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


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

            var previousGoals = _onOneSessionGolasService.GetAllOneOnOneSessionPreviousGoalsByUserId(assignToViewModel.id);

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

            var previousGoals = _onOneSessionGolasService.GetAllOneOnOneSessionNextGoalsByUserId(assignToViewModel.id);

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

            var previousGoals = _onOneSessionGolasService.GetAllOneOnOneSessionPreviousGoalsByUserId(assignToViewModel.id);

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
        public IHttpActionResult CreateSession([FromBody]OneOnOneSessionViewModel oneOnOneSessionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oneOnOneSession = Mapper.Map<OneOnOneSession>(oneOnOneSessionViewModel);

            try
            {
                _oneOnOneSessionsService.CreateSession(oneOnOneSession,CurrentUser);
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

            OneOnOneSession session = null;
            try
            {
                session = _oneOnOneSessionsService.GetById(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            OneOnOneSessionViewModel oneOnOneSessionViewModel = Mapper.Map<OneOnOneSessionViewModel>(session);
            oneOnOneSessionViewModel.Animator.Avatar = CommonHelperMethods.ResolveAvatarPath(oneOnOneSessionViewModel.Animator.Avatar);
            oneOnOneSessionViewModel.Attendee.Avatar = CommonHelperMethods.ResolveAvatarPath(oneOnOneSessionViewModel.Attendee.Avatar);

            return Ok(oneOnOneSessionViewModel);
        }


        [HttpPost]
        [Route("GetSessions")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetSessions()
        {
            IList<OneOnOneSession> oneOnOneSessions = null;

            try
            {
                oneOnOneSessions = _oneOnOneSessionsService.GetAll();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ex.Message);
            }

            List<ListOneOnOneSessionViewModel> oneOnOneSessionsViewModel = new List<ListOneOnOneSessionViewModel>();

            foreach (var item in oneOnOneSessions)
            {
                ListOneOnOneSessionViewModel listOneOnOneSessionViewModel = Mapper.Map<ListOneOnOneSessionViewModel>(item);
                listOneOnOneSessionViewModel.Animator.Avatar = CommonHelperMethods.ResolveAvatarPath(listOneOnOneSessionViewModel.Animator.Avatar);
                listOneOnOneSessionViewModel.Attendee.Avatar = CommonHelperMethods.ResolveAvatarPath(listOneOnOneSessionViewModel.Attendee.Avatar);

                oneOnOneSessionsViewModel.Add(listOneOnOneSessionViewModel);
            }

            return Ok(oneOnOneSessionsViewModel.ToArray());

        }
   }
}


