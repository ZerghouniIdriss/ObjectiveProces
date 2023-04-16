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
    [RoutePrefix("api/Content")]
    public class ContentController : BaseController
    {
        private IContentsService _ContentsService = null;

        public ContentController(IUserService usersService, IContentsService ContentsService) : base(usersService)
        {
            this._ContentsService = ContentsService;
        }

        [HttpPost]
        [Route("Add")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Add([FromBody]ContentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Content content = new Content() {  Code = viewmodel.code,Text = viewmodel.text}; 

                _ContentsService.Add(content);

                return Ok(Mapper.Map<ContentViewModel>(content));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        [Route("Edit")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Edit([FromBody]ContentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Content Content = new Content() { Code = viewmodel.code, Text = viewmodel.text };
                _ContentsService.Update(Content);
                
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
        [Route("DeleteContent")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete([FromBody]ContentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
     
            try
            {
                Content Content = new Content() { Code = viewmodel.code, Text = viewmodel.text };
                _ContentsService.Remove(Content);
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
