using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using OkrsEntreprise.Api.Models;
using System.Linq;
using OkrsEntreprise.Api.App_Start;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Activity")]
    public class ActvitiyController : BaseController
    {
        private IActivityService _activityService = null;

        public ActvitiyController(IUserService usersService, IActivityService activityService)
            : base(usersService)
        {
            this._activityService = activityService;
        }


        [HttpPost]
        [Route("All")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search()
        {
            var activities = _activityService.GetAll();
            var activitiesModels = new List<ActivityViewModel>();
            foreach (var item in activities)
            {
                ActivityViewModel activityViewModel = Mapper.Map<ActivityViewModel>(item);
                activityViewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(activityViewModel.Avatar);

                activitiesModels.Add(activityViewModel);
            }
            return Ok(activitiesModels.OrderByDescending(a => a.ActivityDate).ToArray());
        }


        //[HttpGet]
        //[Route("Get")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public IHttpActionResult Search(string filter)
        //{        
        //    var orkType = (OrkType)Enum.Parse(typeof(OrkType), filter, true);
        //    IList<Goal> goals  = null;
        //    switch (orkType)
        //    {
        //        case OrkType.OnTrack:
        //            goals = _dashboardService.GetOnTrackGoals();
        //            break;
        //        case OrkType.AtRisk:
        //            goals =  _dashboardService.GetAtRiskGoals();
        //            break;
        //        case OrkType.Delayed:
        //            goals = _dashboardService.GetDelayedGoals();
        //            break;
        //    }
        //    var goalModels = new List<ListGoalFullLoadViewModel>();
        //    foreach (var item in goals)
        //    {
        //        goalModels.Add(Mapper.Map<ListGoalFullLoadViewModel>(item));
        //    }
        //    return Ok(goalModels.ToArray());
        //}

        //[HttpGet]
        //[Route("Overall")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public IHttpActionResult GetOverallProgress()
        //{
        //    return Ok(_dashboardService.GetOverallProgress());
        //}

        //[HttpGet]
        //[Route("ByProgress")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public IHttpActionResult GetByProgress()
        //{
        //    return Ok(_dashboardService.GetOrksByProgress());
        //}
    }
}
