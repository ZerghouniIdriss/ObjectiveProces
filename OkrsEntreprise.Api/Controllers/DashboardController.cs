using AutoMapper;
using OkrsEntreprise.Api.App_Start;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : BaseController
    {
        private IDashboardService _dashboardService = null;
        private IActivityService _activityService = null;
        private IGoalsService _goalService = null;
        public DashboardController(IUserService usersService, IDashboardService dashboardService, IActivityService activityService, IGoalsService goalService)
            : base(usersService)
        {
            _dashboardService = dashboardService;
            _activityService = activityService;
            _goalService = goalService;
        }

        [HttpGet]
        [Route("Get")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(string filter)
        {
            var orkType = (OrkType)Enum.Parse(typeof(OrkType), filter, true);
            IList<Goal> goals = null;
            IList<Goal> allGoals = _goalService.GetAll();
            switch (orkType)
            {
                case OrkType.OnTrack:
                    goals = _dashboardService.GetOnTrackGoals(allGoals);
                    break;
                case OrkType.AtRisk:
                    goals = _dashboardService.GetAtRiskGoals(allGoals);
                    break;
                case OrkType.Delayed:
                    goals = _dashboardService.GetDelayedGoals(allGoals);
                    break;
            }
            var goalModels = new List<GoalViewModel>();
            foreach (var item in goals)
            {
                goalModels.Add(Mapper.Map<GoalViewModel>(item));
            }
            return Ok(goalModels.ToArray());
        }

        [HttpGet]
        [Route("Overall")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetOverallProgress()
        {
            return Ok(_dashboardService.GetOverallProgress());
        }

        [HttpGet]
        [Route("MyProgress")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetMyProgress()
        {

            return Ok(_dashboardService.GetOverallProgressByUser(CurrentUser));
        }

        [HttpGet]
        [Route("ByProgress")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetByProgress()
        {
            return Ok(_dashboardService.GetOrksByProgress());
        }

        [HttpPost]
        [Route("RecentActivities")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RecentActivities()
        {
            int take = 100;
            var activities = _activityService.GetAll(take);
            var activitiesModels = new List<ActivityViewModel>();
            foreach (var item in activities)
            {
                ActivityViewModel activityViewModel = Mapper.Map<ActivityViewModel>(item);
                activityViewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(activityViewModel.Avatar);

                activitiesModels.Add(activityViewModel);
            }
            return Ok(activitiesModels.OrderByDescending(a => a.ActivityDate).ToArray());
        }
    }
}
