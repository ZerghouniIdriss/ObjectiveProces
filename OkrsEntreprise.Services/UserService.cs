using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.VisualBasic.ApplicationServices;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public class UserService : UserManager<ApplicationUser, long>, IUserService
    {

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;


        public UserService(IRoleRepository roleRepository) : base(new UserRepository())
        {
            _userRepository = new UserRepository();
            _roleRepository = roleRepository;            
        }

        public override Task<IdentityResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override Task<IdentityResult> AddPasswordAsync(long userId, string password)
        {
            return base.AddPasswordAsync(userId, password);
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await base.CreateAsync(user, password);
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return await base.CreateAsync(user);
        }

        public override async Task<IdentityResult> ConfirmEmailAsync(long userId, string code)
        {
            return await base.ConfirmEmailAsync(userId, code);
        }

        public override async Task<bool> IsEmailConfirmedAsync(long id)
        {
            return await base.IsEmailConfirmedAsync(id);
        }

        public override async Task<IList<string>> GetValidTwoFactorProvidersAsync(long userId)
        {
            return await base.GetValidTwoFactorProvidersAsync(userId);
        }

        public override async Task<IdentityResult> ResetPasswordAsync(long id, string code, string password)
        {
            return await base.ResetPasswordAsync(id, code, password);
        }

        public override async Task<ApplicationUser> FindByNameAsync(string email)
        {
            return await base.FindByNameAsync(email);
        }

        public override async Task<ApplicationUser> FindByIdAsync(long Id)
        {
            return base.FindByIdAsync(Id).Result;
        }

        public override Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return base.FindAsync(login);
        }

        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return base.FindAsync(userName, password);
        }


        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            return await base.DeleteAsync(user);
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await base.UpdateAsync(user);
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return !String.IsNullOrEmpty((await base.FindByIdAsync(user.Id)).PasswordHash);
        }



        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            await base.AddToRoleAsync(user.Id, roleName);
        }


        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return await base.IsInRoleAsync(user.Id, roleName);
        }

        public void AddTeamsToUser(ApplicationUser user, Team[] teams)
        {
            _userRepository.AddTeamsToUser(user, teams);
        }


        public void AddRoleToUser(ApplicationUser user, ApplicationRole role)
        {
            _userRepository.AddRoleToUser(user, role);
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await base.UpdateAsync(user);
        }

        public void EditFirstName(ApplicationUser user)
        {
            ApplicationUser toUpdate = _userRepository.FindByIdAsync(user.Id).Result;

            if (toUpdate != null)
            {
                toUpdate.FirstName = user.FirstName;
                _userRepository.UpdateAsync(toUpdate);
            }
            else
            {
                throw new Exception("User not found");
            }
        }


        public List<ApplicationUser> GetAllUsers()
        {
            return Users.ToList();
        }

        public void EditLastName(ApplicationUser user)
        {
            ApplicationUser toUpdate = _userRepository.FindByIdAsync(user.Id).Result;

            if (toUpdate != null)
            {
                toUpdate.LastName = user.LastName;
                _userRepository.UpdateAsync(toUpdate);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public void EditEmail(ApplicationUser user)
        {
            ApplicationUser toUpdate = _userRepository.FindByIdAsync(user.Id).Result;

            if (toUpdate != null)
            {
                toUpdate.Email = user.Email;
                _userRepository.UpdateAsync(toUpdate);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public void EditAvatar(ApplicationUser user)
        {
            ApplicationUser toUpdate = _userRepository.FindByIdAsync(user.Id).Result;

            if (toUpdate != null)
            {
                toUpdate.Avatar = user.Avatar;
                _userRepository.UpdateAsync(toUpdate);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public void EditManager(ApplicationUser user)
        {
            ApplicationUser toUpdate = _userRepository.FindByIdAsync(user.Id).Result;

            if (toUpdate != null)
            {
                toUpdate.ManagerId = user.ManagerId;
                _userRepository.UpdateAsync(toUpdate);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public void RemoveTeamToUser(Team team, ApplicationUser user)
        {
            _userRepository.RemoveTeamToUser(team, user);
        }

        public void RemoveRoleFromUser(ApplicationRole applicationRole, ApplicationUser user)
        {
            _userRepository.RemoveRoleFromUser(applicationRole, user);
        }

        public ApplicationUser GetById(long userId)
        {
            return _userRepository.FindByIdAsync(userId).Result;
        }

        public void EditTenant(ApplicationUser applicationUser)
        {
            _userRepository.EditTenant(applicationUser);
        }

        public List<ApplicationRole> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public static UserService Create(IdentityFactoryOptions<UserService> options, IOwinContext context)
        {
            // Allows cors for the /token endpoint this is different from webapi endpoints. 
            //context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });  // <-- This is the line you need

            var manager = new UserService(new RoleRepository());
            //Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser,long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

           // Configure validation logic for passwords
           manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //     manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser,long>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        }

        public Task<List<ApplicationUser>> GetMostRecognized(int top)
        {
            return _userRepository.GetMostRecognized(top);
        }

        public async Task InviteUsers(List<ApplicationUser> inviteesUsers)
        { 
            foreach (var invitees in inviteesUsers)
            {
                await CreateAsync(invitees);
            }

            List<ApplicationUser> createdUsers = new List<ApplicationUser>();
            foreach (var invitees in inviteesUsers)
            {
                var tempuser = FindByEmailAsync(invitees.Email).Result;

                await SendEmailAsync(tempuser.Id, "this the subject", "And this is the body");
            }
             
        }
    }


    public interface IUserService :IDisposable
    {
         
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IdentityResult> ConfirmEmailAsync(long userId, string code);
        Task<bool> IsEmailConfirmedAsync(long id);
        Task<IdentityResult> AddLoginAsync(long id, UserLoginInfo login);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(long userId);
        Task<IdentityResult> ResetPasswordAsync(long id, string code, string password);
        Task<ApplicationUser> FindByNameAsync(string email);
        Task<ApplicationUser> FindByIdAsync(long id);
        Task<IdentityResult> DeleteAsync(ApplicationUser user);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task SetPasswordHashAsync(ApplicationUser user, string passwordHash);
        Task<IdentityResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(long userId, string password);
        Task<ApplicationUser> FindAsync(UserLoginInfo login);
        Task<ApplicationUser> FindAsync(string userName, string password);
        List<ApplicationUser> GetAllUsers();

        void AddRoleToUser(ApplicationUser user, ApplicationRole role);

        void AddTeamsToUser(ApplicationUser user, Team[] teams);

        void EditFirstName(ApplicationUser user);

        void EditLastName(ApplicationUser user);

        void EditEmail(ApplicationUser user);

        void EditAvatar(ApplicationUser user);
        void EditManager(ApplicationUser user);
        void RemoveTeamToUser(Team team, ApplicationUser user);

        void RemoveRoleFromUser(ApplicationRole applicationRole, ApplicationUser user);

        List<ApplicationRole> GetAllRoles();

        ApplicationUser GetById(long currentUserId);

        void EditTenant(ApplicationUser applicationUser);

        Task<List<ApplicationUser>> GetMostRecognized(int top);
        Task InviteUsers(List<ApplicationUser> inviteesUser);
    }


}
