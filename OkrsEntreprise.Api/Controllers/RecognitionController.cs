using AutoMapper;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Recognition")]
    public class RecognitionController : BaseController
    {
        IRecognitionService _recognitionService = null;

        public RecognitionController(IUserService usersService, IRecognitionService recognitionService)
            : base(usersService)
        {
            _recognitionService = recognitionService;
        }

        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(CreateRecognitionViewModelViewModel recognitionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recognition = new Recognition
            {
                Text = recognitionViewModel.text,
                GiverId = CurrentUser.Id,
                GoalId = recognitionViewModel.goal
            };

            try
            {
                //add recognition
                _recognitionService.Add(recognition);

                if (recognitionViewModel.users != null)
                {
                    List<ApplicationUser> users = new List<ApplicationUser>();
                    foreach (var item in recognitionViewModel.users)
                    {
                        users.Add(new ApplicationUser { Id = item });
                    }

                    _recognitionService.AddRecognitionToUsers(recognition, users.ToArray());
                }

                if (recognitionViewModel.teams != null)
                {
                    List<Team> teams = new List<Team>();
                    foreach (var item in recognitionViewModel.teams)
                    {
                        teams.Add(new Team { Id = item });
                    }

                    _recognitionService.AddRecognitionToTeams(recognition, teams.ToArray());
                }

                //if (recognitionViewModel.goals != null)
                //{
                //    List<Goal> goals = new List<Goal>();
                //    foreach (var item in recognitionViewModel.goals)
                //    {
                //        goals.Add(Mapper.Map<Goal>(item));
                //    }

                //    _recognitionService.AddRecognitionToGoals(recognition, goals.ToArray());
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            var result = _recognitionService.GetById(recognition.Id);
            return Ok(Mapper.Map<RecognitionViewModel>(result));
        }

        [Route("All")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult All()
        {
            try
            {
                //add recognition
                var recognitions = _recognitionService.GetAll();
                List<RecognitionViewModel> recognitionViewModels = new List<RecognitionViewModel>();

                foreach (var item in recognitions)
                {
                    recognitionViewModels.Add(Mapper.Map<RecognitionViewModel>(item));
                }

                return Ok(recognitionViewModels);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}