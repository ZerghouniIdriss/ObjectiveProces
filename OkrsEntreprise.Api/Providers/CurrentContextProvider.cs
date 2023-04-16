using System;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OkrsEntreprise.Framework;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;

namespace OkrsEntreprise.Api
{
    public class CurrentContextProvider : ICurrentContextProvider<ApplicationUser>
    {

        private IUserService _usersService;
        public CurrentContextProvider(IUserService usersService)
        {
            this._usersService = usersService;
        }

        public  ApplicationUser GetCurrentUser()
        {
            return _usersService.FindByIdAsync(Convert.ToInt64(HttpContext.Current.User.Identity.GetUserId())).Result;
             
        }

        
    }
}
