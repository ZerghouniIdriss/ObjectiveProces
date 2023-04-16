using AutoMapper;
using OkrsEntreprise.Api.App_Start;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Goal")]
    public class GoalController : BaseController
    {
        private IGoalsService _goalsService = null;
        private IKeyResultsService _keyResultsService = null;
        private IUserGoalsService _userGoalsService = null;
        private ITeamService _teamService = null;
        private ITeamGoalsService _teamGoalsService = null;


        public GoalController(IUserService usersService, IGoalsService goalsService, IKeyResultsService keyResultsService, IUserGoalsService userGoalsService, ITeamService teamService, ITeamGoalsService teamGoalsService) : base(usersService)
        {
            this._goalsService = goalsService;
            this._keyResultsService = keyResultsService;
            this._userGoalsService = userGoalsService;
            this._teamService = teamService;
            this._teamGoalsService = teamGoalsService;
        }

        // POST: Goals
        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(GoalFullLoadViewModel goalFullLoadViewModel)
        {
            var goals = _goalsService.GetAllFullLoad();
            var goalModels = new List<ListGoalFullLoadViewModel>();

            foreach (var item in goals)
            {
                ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                foreach (var team in fullLoadViewModel.teams)
                {
                    team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                }

                foreach (var user in fullLoadViewModel.users)
                {
                    user.Avatar = CommonHelperMethods.ResolveAvatarPath(user.Avatar);
                }

                goalModels.Add(fullLoadViewModel);
            }

            return Ok(goalModels.ToArray());
        }

        // POST: Goals
        [HttpPost]
        [Route("GetClosedGoals")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetClosedGoalse(GoalFullLoadViewModel goalFullLoadViewModel)
        {
            var goals = _goalsService.GetAllClosedGoals();
            var goalModels = new List<ListGoalFullLoadViewModel>();

            foreach (var item in goals)
            {
                ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                foreach (var team in fullLoadViewModel.teams)
                {
                    team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                }

                foreach (var user in fullLoadViewModel.users)
                {
                    user.Avatar = CommonHelperMethods.ResolveAvatarPath(user.Avatar);
                }

                goalModels.Add(fullLoadViewModel);
            }

            return Ok(goalModels.ToArray());
        }

        // POST: Goals
        [HttpPost]
        [Route("ForAutocomplete")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult ForAutocomplete(GoalFullLoadViewModel goalFullLoadViewModel)
        {
            var goals = _goalsService.GetAllFullLoad();
            var goalModels = new List<AutocompleteGoalViewModel>();

            foreach (var item in goals)
            {
                goalModels.Add(Mapper.Map<AutocompleteGoalViewModel>(item));
            }

            return Ok(goalModels.ToArray());
        }


        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(GoalFullLoadViewModel goalFullLoadViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var goal = new Goal()
            {
                Title = goalFullLoadViewModel.title,
                Detail = goalFullLoadViewModel.description,
                IsPrivate = goalFullLoadViewModel.isprivate,
                GoalStatusId = goalFullLoadViewModel.status.id,
                GoalCategoryId = goalFullLoadViewModel.category.id,
                Deadline = goalFullLoadViewModel.duedate,
                Priority = goalFullLoadViewModel.priority,
            };

            try
            {
                //add goal and key results if any
                if (goalFullLoadViewModel.keyresults != null && goalFullLoadViewModel.keyresults.Length > 0)
                {
                    KeyResult[] keyResults = goalFullLoadViewModel.keyresults.Select(k => new KeyResult()
                    {
                        Title = k.title,
                        Status = k.status,
                        Goal = goal
                    }).ToArray();

                    goal.KeyResults = keyResults;
                }

                _goalsService.AddGoal(goal);

                ApplicationUser[] users = goalFullLoadViewModel.assignees.Where(a => !a.isteam).Select(a => new ApplicationUser() { Id = a.id, UserName = a.name }).ToArray();
                if (users.Length > 0)
                {
                    _userGoalsService.AddGoalToUser(goal, users);
                }

                Team[] teams = goalFullLoadViewModel.assignees.Where(a => a.isteam).Select(a => new Team() { Id = a.id, Name = a.name }).ToArray();
                if (teams.Length > 0)
                {
                    _teamGoalsService.AddGoalToTeams(goal, teams);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok(Mapper.Map<GoalFullLoadViewModel>(goal));
        }


        //[Route("Create")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public IHttpActionResult Create(GoalFullLoadViewModel goalFullLoadViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var goal = new Goal()
        //    {
        //        Title = goalFullLoadViewModel.title,
        //        Detail = goalFullLoadViewModel.description,
        //        IsPrivate = goalFullLoadViewModel.isprivate,
        //        GoalStatusId = goalFullLoadViewModel.status.id,
        //        GoalCategoryId = goalFullLoadViewModel.category.id,
        //        Deadline = goalFullLoadViewModel.duedate,
        //        Priority = goalFullLoadViewModel.priority,
        //        //EntityCreator = CurrentUser
        //    };

        //    //add goal and key results if any
        //    if (goalFullLoadViewModel.keyresults != null && goalFullLoadViewModel.keyresults.Length > 0)
        //    {
        //        KeyResult[] keyResults = goalFullLoadViewModel.keyresults.Select(k => new KeyResult()
        //        {
        //            Title = k.title,
        //            Status = k.status,
        //            Goal = goal
        //        }).ToArray();

        //        goal.KeyResults = keyResults;
        //    }

        //    ApplicationUser[] users = goalFullLoadViewModel.assignees.Where(a => !a.isteam).Select(a => new ApplicationUser() { Id = a.id, UserName = a.name }).ToArray();
        //    if (users.Length > 0)
        //    {
        //        goal.Users = users;
        //    }

        //    Team[] teams = goalFullLoadViewModel.assignees.Where(a => a.isteam).Select(a => new Team() { Id = a.id, Name = a.name }).ToArray();
        //    if (teams.Length > 0)
        //    { 
        //        goal.Teams = teams;
        //    }
        //    try
        //    {
        //        _goalsService.Add(goal);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return BadRequest(ModelState);
        //    }

        //    return Ok(Mapper.Map<GoalFullLoadViewModel>(goal));
        //}


        [Route("Edit")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Edit(GoalFullLoadViewModel goalFullLoadViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (goalFullLoadViewModel.id == 0)
            {
                return BadRequest();
            }
            try
            {
                var goalToEdit = _goalsService.GetByGaolId(goalFullLoadViewModel.id);
                goalToEdit.Title = goalFullLoadViewModel.title;
                _goalsService.Update(goalToEdit);
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
                return BadRequest("Invalid request");
            }

            try
            {
                Goal goal = _goalsService.GetByGaolId(id);

                if (goal != null)
                {
                    GoalFullLoadViewModel goalFullLoadViewModel = Mapper.Map<GoalFullLoadViewModel>(goal);

                    foreach (var team in goalFullLoadViewModel.teams)
                    {
                        team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                    }

                    foreach (var user in goalFullLoadViewModel.users)
                    {
                        user.Avatar = CommonHelperMethods.ResolveAvatarPath(user.Avatar);
                    }

                    foreach (var comment in goalFullLoadViewModel.comments)
                    {
                        comment.user.Avatar = CommonHelperMethods.ResolveAvatarPath(comment.user.Avatar);
                    }

                    return Ok(goalFullLoadViewModel);
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
        [Route("Delete")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete([FromBody]long id)
        {
            //int id = 0;
            if (id <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                _goalsService.RemoveGoals(new Goal() { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("EditTitle")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditTitle([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(goalviewmodel.title))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateTitle(goalviewmodel.id, goalviewmodel.title);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditProgress")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditProgress([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateProgress(goalviewmodel.id, goalviewmodel.progress);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditStatus")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditStatus([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (goalviewmodel.status == null)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateStatus(goalviewmodel.id, goalviewmodel.status.id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditPriority")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditPriority([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdatePriority(goalviewmodel.id, goalviewmodel.priority);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditCategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditCategory([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (goalviewmodel.category == null)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateCategory(goalviewmodel.id, goalviewmodel.category.id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditIsPrivate")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditIsPrivate([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateIsPrivate(goalviewmodel.id, goalviewmodel.isprivate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditDueDate")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditDueDate([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }


            try
            {
                _goalsService.UpdateDueDate(goalviewmodel.id, goalviewmodel.duedate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditDes")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditDes([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }


            try
            {
                _goalsService.UpdateDes(goalviewmodel.id, goalviewmodel.description);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditGoalIndicator")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditIndicator(long id, long indicatorId, long indicatorValueId)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }


            try
            {
                _goalsService.UpdateIndicator(id, indicatorId, indicatorValueId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditIsOpen")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditIsOpen([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }


            try
            {
                _goalsService.UpdateIsOpen(goalviewmodel.id, goalviewmodel.isopen);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("EditPar")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditPar([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.UpdateParent(goalviewmodel.id, goalviewmodel.parent != null ? new Nullable<long>(goalviewmodel.parent.id) : null);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        [HttpPost]
        [Route("EditAssignees")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditAssignees([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                Goal goal = new Goal() { Id = goalviewmodel.id };
                ApplicationUser[] users = goalviewmodel.assignees.Where(a => !a.isteam).Select(a => new ApplicationUser() { Id = a.id, UserName = a.name }).ToArray();
                if (users.Length > 0)
                {
                    _userGoalsService.AddGoalToUser(goal, users);
                }

                Team[] teams = goalviewmodel.assignees.Where(a => a.isteam).Select(a => new Team() { Id = a.id, Name = a.name }).ToArray();
                if (teams.Length > 0)
                {
                    _teamGoalsService.AddGoalToTeams(goal, teams);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("AddSubGoal")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AddSubGoal([FromBody]ParentChildGoalViewModel parentChildGoalViewModel)
        {
            if (parentChildGoalViewModel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (parentChildGoalViewModel.parentid <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.AddSubGoal(parentChildGoalViewModel.parentid, parentChildGoalViewModel.id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("DeleteSubGoal")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSubGoal([FromBody]ParentChildGoalViewModel parentChildGoalViewModel)
        {
            if (parentChildGoalViewModel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (parentChildGoalViewModel.parentid <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalsService.DeleteSubGoal(parentChildGoalViewModel.parentid, parentChildGoalViewModel.id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        [HttpPost]
        [Route("AssignMe")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssignMe([FromBody]GoalFullLoadViewModel goalviewmodel)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Invalid request");
            //    return BadRequest(ModelState);
            //}

            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                Goal goal = new Goal() { Id = goalviewmodel.id };
                _userGoalsService.AddGoalToUser(goal, new ApplicationUser[] { new ApplicationUser() { Id = CurrentUser.Id, UserName = CurrentUser.UserName } });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }

        [HttpPost]
        [Route("RemoveMe")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RemoveMe([FromBody]GoalFullLoadViewModel goalviewmodel)
        {


            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }


            try
            {
                Goal goal = new Goal() { Id = goalviewmodel.id };
                _userGoalsService.RemoveGoalToUser(goal, CurrentUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }



        [HttpPost]
        [Route("GetGoalListMap")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetGoalListMap()
        {
            var goals = this._goalsService.GetGoalsMap();
            var goalViewModels = new List<GoalMapViewModel>();

            foreach (var item in goals)
            {
                GoalMapViewModel viewmodel = Mapper.Map<GoalMapViewModel>(item);
                foreach (var team in viewmodel.teams)
                {
                    team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                }

                foreach (var user in viewmodel.users)
                {
                    user.Avatar = CommonHelperMethods.ResolveAvatarPath(user.Avatar);
                }
                goalViewModels.Add(viewmodel);
            }

            return Ok(goalViewModels.ToArray());
        }


        [HttpPost]
        [Route("UnassignUser")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UnassignUser([FromBody]GoalFullLoadViewModel goalviewmodel)
        {

            if (goalviewmodel.id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (goalviewmodel.users.Length <= 0 && goalviewmodel.teams.Length <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }
            try
            {
                Goal goal = new Goal() { Id = goalviewmodel.id };

                if (goalviewmodel.users.Length > 0)
                {
                    ApplicationUser user = new ApplicationUser() { Id = goalviewmodel.users[0].id };

                    _userGoalsService.RemoveGoalToUser(goal, user);
                }

                if (goalviewmodel.teams.Length > 0)
                {
                    Team team = new Team() { Id = goalviewmodel.teams[0].id };
                    _teamGoalsService.RemoveGoalForTeam(goal, team);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();

        }


        // POST: MostRecognized
        [HttpPost]
        [Route("MostRecognized")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> MostRecognized()
        {
            var goals = await _goalsService.MostRecognized(3);
            var goalModels = new List<ListGoalFullLoadViewModel>();

            foreach (var item in goals)
            {
                ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                foreach (var team in fullLoadViewModel.teams)
                {
                    team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                }

                foreach (var user in fullLoadViewModel.users)
                {
                    user.Avatar = CommonHelperMethods.ResolveAvatarPath(user.Avatar);
                }

                goalModels.Add(fullLoadViewModel);
            }

            return Ok(goalModels.ToArray());
        }

    }
}
