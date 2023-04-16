using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public class SignInService : SignInManager<Model.Entities.ApplicationUser, long>, ISignInService
    {
    
        public SignInService(IUserService usersService, IAuthenticationManager authenticationManager) : base(usersService as UserManager<ApplicationUser, long>, authenticationManager)
        {
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe, bool shouldLockout)
        {
            return await base.PasswordSignInAsync(email, password, rememberMe, shouldLockout);
        }

        public override async Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            return await base.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser);
        }

        public  async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent)
        {
            return await base.ExternalSignInAsync(loginInfo, isPersistent);
        }

        public async Task<long> GetVerifiedUserIdAsync()
        {
            return await base.GetVerifiedUserIdAsync();
        }

    }

    public interface ISignInService
    {
        Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe, bool shouldLockout);
        Task<bool> HasBeenVerifiedAsync();
        Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);
        Task SignInAsync(Model.Entities.ApplicationUser user, bool isPersistent, bool rememberBrowser);
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent);
        void Dispose();
        Task<long> GetVerifiedUserIdAsync();
        Task<bool> SendTwoFactorCodeAsync(string selectedProvider);
    }
}