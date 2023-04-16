using System.Web.Http;
using Microsoft.AspNet.Identity;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;

namespace OkrsEntreprise.Api.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IUserService _userService = null;
        
        private ApplicationUser _currentUser  ;
        
        public BaseController(IUserService usersService )
        {
            this._userService = usersService;
        }

      
        public ApplicationUser CurrentUser
        {
            get
            {
                var currentUserId =  User.Identity.GetUserId<long>();
                var currentuser = _userService.GetById(currentUserId); 
                return currentuser;
            }
            set
            {
                _currentUser = value;
            }
        }

        public long CurrentUserId
        {
            get
            {
             return User.Identity.GetUserId<long>();
            }
            set
            {
                CurrentUserId = value;
            }
        }

    }
}
