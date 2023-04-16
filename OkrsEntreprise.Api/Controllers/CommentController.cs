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
    [RoutePrefix("api/Comment")]
    public class CommentController : BaseController
    {
        private ICommentsService _commentsService = null;
        private IGoalsService _goalsService = null;

        public CommentController(IUserService usersService, ICommentsService commentsService, IGoalsService goalsService) : base(usersService)
        {
            _commentsService = commentsService;

            _goalsService = goalsService;
    }

        [HttpPost]
        [Route("AddComment")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddComment([FromBody]CommentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Comment comment = new Comment()
                {
                    Text = viewmodel.text,
                    //EntityCreator = new ApplicationUser() {Id = CurrentUser.Id},
                    GoalId = viewmodel.goalid
                };
                _commentsService.Add(comment);

                return Ok(Mapper.Map<CommentViewModel>(comment));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("EditComment")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditComment([FromBody]CommentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Comment comment = new Comment() { Id = viewmodel.id, Text = viewmodel.text };
                Comment returnVal = _commentsService.EditComment(comment, CurrentUser);

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
        [Route("DeleteComment")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteComment([FromBody]CommentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Comment comment = new Comment() { Id = viewmodel.id};
                Comment returnVal = _commentsService.DeleteComment(comment);

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
