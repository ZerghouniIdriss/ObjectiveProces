using AutoMapper;
using OkrsEntreprise.Services;
using OkrsEntreprise.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using OkrsEntreprise.Model.Entities;
using System.Threading.Tasks;
using System.Web;
using OkrsEntreprise.Framework.Photo;
using System.Web.Hosting;
using OkrsEntreprise.Api.App_Start;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Team")]
    public class TeamController : BaseController
    {
        private ITeamService _teamService = null;
        private ITeamGoalsService _teamGoalsService = null;

        public TeamController(IUserService userService, ITeamService teamService, ITeamGoalsService teamGoalsService) : base(userService)
        {
            this._teamService = teamService;
            this._teamGoalsService = teamGoalsService;
        }

        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(TeamViewModel teamViewModel)
        {
            var teams = this._teamService.GetAll();
            var teamViewModels = new List<TeamViewModel>();

            foreach (var item in teams)
            {
                TeamViewModel viewmodel = Mapper.Map<TeamViewModel>(item);
                viewmodel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewmodel.Avatar);

                teamViewModels.Add(viewmodel);
            }

            return Ok(teamViewModels.ToArray());
        }

        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(TeamViewModel teamViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Team team = Mapper.Map<Team>(teamViewModel);

            try
            {
                _teamService.AddTeam(team);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        // POST: TeamList
        [HttpPost]
        [Route("TeamList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult TeamList(TeamViewModel teamViewModel)
        {
            var teams = _teamService.GetAll();
            var teamModels = new List<ListTeamViewModel>();

            foreach (var team in teams)
            {
                ListTeamViewModel viewModel = Mapper.Map<ListTeamViewModel>(team);
                viewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewModel.Avatar);

                teamModels.Add(viewModel);
            }

            return Ok(teamModels.ToArray());
        }

        [HttpPost]
        [Route("AssigneeList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssigneeList(TeamViewModel teamViewModel)
        {
            var teams = _teamService.GetAll();
            var teamModels = new List<AssignToViewModel>();

            foreach (var team in teams)
            {
                AssignToViewModel viewModel = Mapper.Map<AssignToViewModel>(team);
                viewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewModel.Avatar);

                teamModels.Add(viewModel);
            }

            return Ok(teamModels.ToArray());
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
                Team team = new Team { Id = id };
                _teamService.Remove(new Team[] { team });

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
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
                Team team = _teamService.GetById(id);

                if (team != null)
                {
                    TeamViewModel teamViewModel = Mapper.Map<TeamViewModel>(team);
                    var goals = _teamGoalsService.GoalByTeamId(id);
                    var goalModels = new List<ListGoalFullLoadViewModel>();

                    foreach (var item in goals)
                    {
                        ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                        foreach (var teamObj in fullLoadViewModel.teams)
                        {
                            teamObj.Avatar = CommonHelperMethods.ResolveAvatarPath(teamObj.Avatar);
                        }

                        foreach (var usr in fullLoadViewModel.users)
                        {
                            usr.Avatar = CommonHelperMethods.ResolveAvatarPath(usr.Avatar);
                        }


                        goalModels.Add(fullLoadViewModel);
                    }
                    teamViewModel.Objectives = goalModels;

                    return Ok(teamViewModel);
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
        [Route("EditName")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditName([FromBody]TeamViewModel team)
        {
            if (team.id <= 0 || string.IsNullOrEmpty(team.name))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _teamService.EditName(new Team() { Id = team.id, Name = team.name });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("EditDescription")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditDescription([FromBody]TeamViewModel team)
        {
            if (team.id <= 0 || string.IsNullOrEmpty(team.description))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _teamService.EditDescription(new Team() { Id = team.id, Description = team.description });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }




        [HttpPost]
        [Route("GetOverallProgress")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetOverallProgress([FromBody]TeamViewModel team)
        {
            if (team.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                //Generate overall progress for the user here
                return Ok(_teamGoalsService.GetOverallProgressByTeam(team.id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("GetAligned")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetAligned([FromBody]TeamViewModel team)
        {
            if (team.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                //Generate overall progress for the user here
                return Ok(_teamGoalsService.GetAlignedGoalByTeam(team.id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("UploadAvatar")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        public async Task<IHttpActionResult> UploadAvatar()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }

            try
            {
                string userId = HttpContext.Current.Request.Form["teamname"];
                long uid;
                if (!long.TryParse(userId, out uid))
                {
                    throw new Exception("Invalid request");
                }

                LocalPhotoManager photoManager = new LocalPhotoManager(HostingEnvironment.MapPath("/AvatarImages"));
                var photos = await photoManager.Add(Request);

                foreach (var photo in photos)
                {
                    _teamService.EditAvatar(new Team() { Id = uid, Avatar = @"\AvatarImages\" + photo.Name });
                    return Ok(CommonHelperMethods.ResolveAvatarPath(@"\AvatarImages\" + photo.Name));
                }

                return BadRequest("Invalid request");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

        }


        [HttpPost]
        [Route("DeleteUser")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteUser([FromBody]TeamViewModel team)
        {
            if (team.id <= 0 || team.Users.Length <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);

            }

            try
            {
                _teamService.RemoveUserFromTeam(new ApplicationUser() { Id = team.Users[0].Id }, new Team() { Id = team.id });

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("AddUser")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddUser([FromBody]TeamViewModel team)
        {
            if (team.id <= 0 || team.Users.Length <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);

            }

            try
            {
                _teamService.AddUserToTeam(new Team() { Id = team.id }, new ApplicationUser[] { new ApplicationUser() { Id = team.Users[0].Id } });
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
