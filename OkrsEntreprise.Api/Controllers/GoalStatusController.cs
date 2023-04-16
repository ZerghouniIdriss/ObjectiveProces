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
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/GoalStatus")]
    public class GoalStatusController : BaseController
    {
        private IGoalStatusService _goalStatusService = null;

        public GoalStatusController(IUserService usersService, IGoalStatusService goalStatusService) : base(usersService)
        {
            this._goalStatusService = goalStatusService;
        }

        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(GoalStatusViewModel goalStatusViewModel)
        {
            var goalstatus = this._goalStatusService.GetAll();
            var goalstatusViewModels = new List<GoalStatusViewModel>();

            foreach (var item in goalstatus)
            {
                goalstatusViewModels.Add(Mapper.Map<GoalStatusViewModel>(item));
            }

            return Ok(goalstatusViewModels.ToArray());
        }



        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(GoalStatusViewModel statusViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GoalStatus status = new GoalStatus() { StatusTitle = statusViewModel.title };

            try
            {
                _goalStatusService.Add(status);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        [HttpPost]
        [Route("Retrieve")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Retrieve([FromBody]long id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                GoalStatus status = _goalStatusService.GetByGoalStatusId(id);

                if (status != null)
                {
                    GoalStatusViewModel goalcategoryViewModel = Mapper.Map<GoalStatusViewModel>(status);

                    return Ok(goalcategoryViewModel);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpPost]
        [Route("UpdateGoalStatus")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UpdateGoalStatus([FromBody]GoalStatusViewModel status)
        {
            if (status.id <= 0 || string.IsNullOrEmpty(status.title))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalStatusService.Update(new GoalStatus() { Id = status.id, StatusTitle = status.title });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpPost]
        [Route("Delete")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete([FromBody]long id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                GoalStatus status = new GoalStatus();
                _goalStatusService.Remove(new GoalStatus() { Id = id, });

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
