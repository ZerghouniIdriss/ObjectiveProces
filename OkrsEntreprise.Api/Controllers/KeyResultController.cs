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

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/KeyResult")]
    public class KeyResultController : BaseController
    {
        private IKeyResultsService _keyresultsService = null;

        public KeyResultController(IUserService usersService, IKeyResultsService keyresultsService) : base(usersService)
        {
            this._keyresultsService = keyresultsService;
        }

        [HttpPost]
        [Route("AddKeyResult")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddKeyResult([FromBody]KeyResultViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Goal goal = new Goal() { Id = viewmodel.goalid };
                KeyResult keyresult = new KeyResult() { Title = viewmodel.title, EntityCreatorId =CurrentUserId};

                _keyresultsService.AddKeyResultToGoal(goal, keyresult);

                return Ok(Mapper.Map<KeyResultViewModel>(keyresult));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("EditKeyResult")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditKeyResult([FromBody]KeyResultViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                KeyResult keyresult = new KeyResult() { Id = viewmodel.id, Title = viewmodel.title };
                KeyResult returnVal = _keyresultsService.EditKeyResult(keyresult);

                if (returnVal == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("DeleteKeyResult")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteKeyResult([FromBody]KeyResultViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                KeyResult keyresult = new KeyResult() { Id = viewmodel.id, Title = viewmodel.title };
                KeyResult returnVal = _keyresultsService.DeleteKeyResult(keyresult);

                if (returnVal == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }

}
