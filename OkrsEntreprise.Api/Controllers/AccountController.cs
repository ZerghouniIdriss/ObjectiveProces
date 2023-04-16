using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using OkrsEntreprise.Api.App_Start;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Framework;
using OkrsEntreprise.Framework.Photo;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OkrsEntreprise.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        //private const string LocalLoginProvider = "Local";
        //private ApplicationUserManager _userManager;

        private IGoalsService _goalsService = null;
        private IUserService _userService = null;
        private ISignInService _signInService = null;
        private IUserGoalsService _userGoalsService = null;
        private ITenantsService _tenatService = null;

        public AccountController()
        {
        }


        public AccountController(IUserService userService, IUserGoalsService userGoalsService, ISignInService signInService, ITenantsService tenantService, IGoalsService goalsService)
        {
            _signInService = signInService;
            _userService = userService;
            _userGoalsService = userGoalsService;
            _tenatService = tenantService;
            _goalsService = goalsService;
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

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }


        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long id = Convert.ToInt64(User.Identity.GetUserId());
            IdentityResult result = await _userService.ChangePasswordAsync(id, model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            long id = Convert.ToInt64(User.Identity.GetUserId());
            IdentityResult result = await _userService.AddPasswordAsync(id, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    ApplicationUser user = await _userService.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(_userService as UserManager<ApplicationUser, long>,
        //           OAuthDefaults.AuthenticationType);
        //        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(_userService as UserManager<ApplicationUser, long>,
        //            CookieAuthenticationDefaults.AuthenticationType);

        //        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
        //        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser();
                user.UserName = model.UName;
                user.Email = model.Email;

                var tenant = _tenatService.GetTenatByCompanyName(model.CName);
                if (tenant != null)
                {
                    user.TenantId = tenant.Id;
                }
                else
                {
                    Tenant tenantObj = new Tenant();
                    tenantObj.Name = model.CName;
                    var insertedTenant = _tenatService.AddTenant(tenantObj);
                    user.TenantId = insertedTenant.Id;
                }

                var identity = RequestContext.Principal.Identity as ClaimsIdentity;
                var userClaim = identity.FindFirst("TenantId");
                if (userClaim != null)
                    identity.RemoveClaim(userClaim);
                identity.AddClaim(new Claim("TenantId", user.TenantId.ToString()));

                IdentityResult result = null;

                try
                {
                    result = await _userService.CreateAsync(user, model.Password);
                    //_userService.EditTenant(new ApplicationUser() { UserName = user.UserName, TenantId = insertedTenant.Id });
                }

                catch (Exception ex)
                {

                }
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await _userService.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await _userService.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        // POST: Users
        [HttpPost]
        [Route("Search")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Search(RegisterBindingModel userViewModel)
        {
            var users = _userService.GetAllUsers();
            var userModels = new List<RegisterBindingModel>();

            foreach (var item in users)
            {
                userModels.Add(Mapper.Map<RegisterBindingModel>(item));
            }

            return Ok(userModels.ToArray());
        }


        // POST: UserList
        [HttpPost]
        [Route("UserList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UserList(RegisterBindingModel userViewModel)
        {
            var users = _userService.GetAllUsers();
            var userModels = new List<ListUserViewModel>();

            foreach (var item in users)
            {
                ListUserViewModel viewModel = Mapper.Map<ListUserViewModel>(item);
                viewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewModel.Avatar);

                userModels.Add(viewModel);
            }

            return Ok(userModels.ToArray());
        }


        [HttpPost]
        [Route("AssigneeList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssigneeList(RegisterBindingModel userViewModel)
        {
            var users = _userService.GetAllUsers();
            var userModels = new List<AssignToViewModel>();

            foreach (var item in users)
            {
                AssignToViewModel viewModel = Mapper.Map<AssignToViewModel>(item);
                viewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewModel.Avatar);

                userModels.Add(viewModel);
            }

            return Ok(userModels.ToArray());
        }


        [HttpPost]
        [Route("Delete")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete([FromBody]long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                ApplicationUser user = new ApplicationUser();
                _userService.DeleteAsync(user);

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
                return BadRequest("Invalid request");
            }

            try
            {
                ApplicationUser user = _userService.FindByIdAsync(id).Result;
                UserViewModel userViewModel = Mapper.Map<UserViewModel>(user);
                if (user != null)
                {

                    var goals = _goalsService.GetAllByUserId(id);
                    var goalModels = new List<ListGoalFullLoadViewModel>();

                    foreach (var item in goals)
                    {
                        ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                        foreach (var team in fullLoadViewModel.teams)
                        {
                            team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                        }

                        foreach (var usr in fullLoadViewModel.users)
                        {
                            usr.Avatar = CommonHelperMethods.ResolveAvatarPath(usr.Avatar);
                        }


                        goalModels.Add(fullLoadViewModel);
                    }
                    userViewModel.Objective = goalModels;


                    var privateGoals = _goalsService.PrivateOnlyByUserId(id);
                    var privateGoalModels = new List<ListGoalFullLoadViewModel>();

                    foreach (var item in privateGoals)
                    {
                        ListGoalFullLoadViewModel fullLoadViewModel = Mapper.Map<ListGoalFullLoadViewModel>(item);

                        foreach (var team in fullLoadViewModel.teams)
                        {
                            team.Avatar = CommonHelperMethods.ResolveAvatarPath(team.Avatar);
                        }

                        foreach (var usr in fullLoadViewModel.users)
                        {
                            usr.Avatar = CommonHelperMethods.ResolveAvatarPath(usr.Avatar);
                        }


                        privateGoalModels.Add(fullLoadViewModel);
                    }


                    userViewModel.Objective = goalModels;
                    userViewModel.PrivateGoals = privateGoalModels;

                    return Ok(userViewModel);
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
        [Route("AssignTeam")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssignTeam([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || user.Teams.Length <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                List<Team> listOfTeams = new List<Team>();

                foreach (var team in user.Teams)
                {
                    listOfTeams.Add(Mapper.Map<Team>(team));
                }
                _userService.AddTeamsToUser(new ApplicationUser() { Id = user.Id }, listOfTeams.ToArray());

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("DeleteTeam")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteTeam([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || user.Teams.Length <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                Team team = Mapper.Map<Team>(user.Teams[0]);
                _userService.RemoveTeamToUser(team, new ApplicationUser() { Id = user.Id });

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("EditFirstName")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditFirstName([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || string.IsNullOrEmpty(user.FirstName))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _userService.EditFirstName(new ApplicationUser() { Id = user.Id, FirstName = user.FirstName });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("EditLastName")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditLastName([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || string.IsNullOrEmpty(user.LastName))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _userService.EditLastName(new ApplicationUser() { Id = user.Id, LastName = user.LastName });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("EditEmail")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditEmail([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                _userService.EditEmail(new ApplicationUser() { Id = user.Id, Email = user.Email });
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("EditManager")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult EditManager([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || user.ManagerId <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            if (user.Id == user.ManagerId)
            {
                ModelState.AddModelError("", "User shouldn't be a manager of himself");
                return BadRequest(ModelState);
            }

            try
            {
                _userService.EditManager(new ApplicationUser() { Id = user.Id, ManagerId = user.ManagerId });
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
        public IHttpActionResult GetOverallProgress([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(_userGoalsService.GetOverallProgressByUser(user.Id));
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
        public IHttpActionResult GetAligned([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_userGoalsService.GetAlignedGoalByUser(user.Id));
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
                string userId = HttpContext.Current.Request.Form["username"];
                long uid;
                if (!long.TryParse(userId, out uid))
                {
                    throw new Exception("Invalid request");
                }

                LocalPhotoManager photoManager = new LocalPhotoManager(HostingEnvironment.MapPath("/AvatarImages"));
                var photos = await photoManager.Add(Request);

                foreach (var photo in photos)
                {
                    _userService.EditAvatar(new ApplicationUser() { Id = uid, Avatar = @"/AvatarImages/" + photo.Name });
                    return Ok(CommonHelperMethods.ResolveAvatarPath(@"/AvatarImages/" + photo.Name));
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
        [Route("RoleList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult RoleList()
        {
            var roles = _userService.GetAllRoles();
            var roleModels = new List<RoleViewModel>();

            foreach (var item in roles)
            {
                roleModels.Add(Mapper.Map<RoleViewModel>(item));
            }

            return Ok(roleModels.ToArray());
        }


        [HttpPost]
        [Route("AssignRole")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult AssignRole([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || user.Roles.Length <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                ApplicationRole role = Mapper.Map<ApplicationRole>(user.Roles[0]);
                _userService.AddRoleToUser(new ApplicationUser() { Id = user.Id }, role);
                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("DeleteRole")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteRole([FromBody]UserEditViewModel user)
        {
            if (user.Id <= 0 || user.Roles.Length <= 0)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                ApplicationRole applicationRole = Mapper.Map<ApplicationRole>(user.Roles[0]);
                _userService.RemoveRoleFromUser(applicationRole, new ApplicationUser() { Id = user.Id });

                return Ok();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userService.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers



        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        [HttpPost]
        [Route("InviteOthers")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult InviteOthers([FromBody]InviteUsersViewModel invitees)
        {
            if (invitees.userId <= 0 || invitees.emails.Length == 0)
            {
                ModelState.AddModelError("", "Invalid request");
                return BadRequest(ModelState);
            }

            try
            {
                var inviteesUser= new List<ApplicationUser>();
                foreach (var inviteesEmail in invitees.emails)
                {
                    inviteesUser.Add(new ApplicationUser()
                        {
                            UserName = inviteesEmail,
                            Email = inviteesEmail
                        }
                    );
                }
                _userService.InviteUsers(inviteesUser);

                ApplicationUser user = _userService.GetById(invitees.userId);
                if (user != null)
                {
                    EmailUtility emailUtility = new EmailUtility();
                    EmailMessageDetails messageDetails = new EmailMessageDetails();
                    messageDetails.Subject = string.Format("Invitation from {0} to join his team.", user.UserName);
                    // messageDetails.Body = File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath("/api/Views/Invitation Email.html")); // Deployment Url path
                    messageDetails.Body = File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath("/Views/Invitation Email.html")); //Local File URL Path

                    emailUtility.SendEmails(invitees.emails, messageDetails);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "User Invalid");
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // POST: GetMostRecognized
        [HttpPost]
        [Route("GetMostRecognized")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetMostRecognized(RegisterBindingModel userViewModel)
        {
            var users = await _userService.GetMostRecognized(3);
            var userModels = new List<RecognitionUserViewModel>();

            foreach (var item in users)
            {
                var viewModel = Mapper.Map<RecognitionUserViewModel>(item);
                viewModel.Avatar = CommonHelperMethods.ResolveAvatarPath(viewModel.Avatar);

                userModels.Add(viewModel);
            }

            return Ok(userModels.ToArray());
        }

    }
}
