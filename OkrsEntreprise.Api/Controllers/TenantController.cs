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
    [RoutePrefix("api/Tenant")]
    public class TenantController : BaseController
    {
        private ITenantsService _tenantsService = null;

        public TenantController(IUserService userService, ITenantsService tenantsService) : base(userService)
        {
            this._tenantsService = tenantsService;
        }



        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(TenantViewModel TenantViewModel)
        {
            var Tenants = this._tenantsService.GetAll();
            var TenantViewModels = new List<TenantViewModel>();

            foreach (var item in Tenants)
            {
                TenantViewModel viewmodel = Mapper.Map<TenantViewModel>(item);

                TenantViewModels.Add(viewmodel);
            }

            return Ok(TenantViewModels.ToArray());
        }



        [Route("Create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Create(TenantViewModel tenatViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tenant tenant = new Tenant() { Name = tenatViewModel.name};

            try
            {
                _tenantsService.AddTenant(tenant);
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
                Tenant tenant = _tenantsService.GetById(id);

                if (tenant != null)
                {
                    TenantViewModel tenantViewModel = Mapper.Map<TenantViewModel>(tenant);
                   
                    return Ok(tenantViewModel);
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
        [Route("UpdateTenat")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UpdateTenat([FromBody]TenantViewModel tenant)
        {
            if (tenant.id <= 0 || string.IsNullOrEmpty(tenant.name))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _tenantsService.UpdateTenant(new Tenant() { Id = tenant.id, Name = tenant.name });
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
        [Route("TenantList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult TenantList(TenantViewModel TenantViewModel)
        {
            var Tenants = this._tenantsService.GetAll();
            var TenantViewModels = new List<TenantViewModel>();

            foreach (var item in Tenants)
            {
                TenantViewModel viewmodel = Mapper.Map<TenantViewModel>(item);

                TenantViewModels.Add(viewmodel);
            }

            return Ok(TenantViewModels.ToArray());
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
                _tenantsService.RemoveTenant(new Tenant() { Id = id,});

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
