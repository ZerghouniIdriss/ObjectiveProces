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
    [RoutePrefix("api/Indicator")]
    public class IndicatorController : BaseController
    {
        private IIndicatorsService _indicatorsService = null;

        public IndicatorController(IUserService userService, IIndicatorsService IndicatorsService) : base(userService)
        {
            this._indicatorsService = IndicatorsService;
        }



        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(IndicatorViewModel indicatorViewModel)
        {

            var indicators = this._indicatorsService.GetAll();

            var indicatorViewModels = new List<IndicatorViewModel>();

            foreach (var item in indicators)
            {
                IndicatorViewModel viewmodel = Mapper.Map<IndicatorViewModel>(item);

                indicatorViewModels.Add(viewmodel);
            }

            return Ok(indicatorViewModels.ToArray());
        }



        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(IndicatorViewModel indicatorViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Indicator indicator = new Indicator() { Id = indicatorViewModel.Id, Title = indicatorViewModel.Title, Type = indicatorViewModel.Type };

            try
            {
                _indicatorsService.Add(indicator);
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
                Indicator Indicator = _indicatorsService.GetById(id);

                if (Indicator != null)
                {
                    IndicatorViewModel IndicatorViewModel = Mapper.Map<IndicatorViewModel>(Indicator);
                   
                    return Ok(IndicatorViewModel);
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
        [Route("Update")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Update([FromBody]IndicatorViewModel Indicator)
        {
            if (string.IsNullOrEmpty(Indicator.Title) || string.IsNullOrEmpty(Indicator.Type))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _indicatorsService.Update(new Indicator() { Id = Indicator.Id, Title = Indicator.Title, Type = Indicator.Type });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // POST: TeamList
        [HttpPost] 
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult List(IndicatorViewModel viewModel)
        {
            var indicators = this._indicatorsService.GetAll(); 
 
            var indicatorViewModels = Mapper.Map<List<IndicatorViewModel>>(indicators); 

            return Ok(indicatorViewModels.ToArray());
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
                Indicator Indicator = new Indicator();
                _indicatorsService.Remove(new Indicator() { Id = id,});

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
