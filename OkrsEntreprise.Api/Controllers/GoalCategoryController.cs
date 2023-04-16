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
using OkrsEntreprise.Model;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/GoalCategory")]
    public class GoalCategoryController : BaseController
    {
        private IGoalCategoriesService _goalCategoriesService = null;

        public GoalCategoryController(IUserService usersService, IGoalCategoriesService goalCategoriesService) : base(usersService)
        {
            this._goalCategoriesService = goalCategoriesService;
        }

        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(GoalCategoryViewModel goalCategoryViewModel)
        {
            var goalcategories = _goalCategoriesService.GetAll();
            var goalcategoryViewModels = new List<GoalCategoryViewModel>();

            foreach (var item in goalcategories)
            {
                goalcategoryViewModels.Add(Mapper.Map<GoalCategoryViewModel>(item));
            }

            return Ok(goalcategoryViewModels.ToArray());
        }



        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(GoalCategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GoalCategory tenant = new GoalCategory() { CategoryTitle = categoryViewModel.title };
          
            try
            {
                _goalCategoriesService.Add(tenant);
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
                GoalCategory category = _goalCategoriesService.GetByGoalCategoryId(id);

                if (category != null)
                {
                    GoalCategoryViewModel goalcategoryViewModel = Mapper.Map<GoalCategoryViewModel>(category);

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
        [Route("UpdateGoalCategory")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UpdateGoalCategory([FromBody]GoalCategoryViewModel categorie)
        {
            if (categorie.id <= 0 || string.IsNullOrEmpty(categorie.title))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _goalCategoriesService.Update(new GoalCategory() { Id = categorie.id, CategoryTitle = categorie.title });
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
                Tenant tenant = new Tenant();
                _goalCategoriesService.Remove(new GoalCategory() { Id = id, });

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
