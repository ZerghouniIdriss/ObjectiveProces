//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using OkrsEntreprise.Model.Entities;
//using OkrsEntreprise.Services;

//namespace OkrsEntreprise.Api
//{
//    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

//    public class ApplicationUserManager  : UserService
//    {
//        //public ApplicationUserManager(IUserStore<ApplicationUser, long> store)
//        //    : base(store)
//        //{
//        //}

//        //public static ApplicationUserManager Create(IdentityFactoryOptions<UserService> options, IOwinContext context)
//        //{
//        //    // Allows cors for the /token endpoint this is different from webapi endpoints. 
//        //    //context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });  // <-- This is the line you need

            
//        //    var manager = new UserService();
//        //    // Configure validation logic for usernames
//        //    //manager.UserValidator = new UserValidator<ApplicationUser>(manager)
//        //    //{
//        //    //    AllowOnlyAlphanumericUserNames = false,
//        //    //    RequireUniqueEmail = true
//        //    //};
//        //    // Configure validation logic for passwords

//        //    manager.PasswordValidator = new PasswordValidator
//        //    {
//        //        RequiredLength = 6,
//        //        RequireNonLetterOrDigit = true,
//        //        RequireDigit = true,
//        //        RequireLowercase = true,
//        //        RequireUppercase = true,
//        //    };
//        //    var dataProtectionProvider = options.DataProtectionProvider;
//        //    if (dataProtectionProvider != null)
//        //    {
//        //        //manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser,long>(dataProtectionProvider.Create("ASP.NET Identity"));
//        //    }
//        //    return manager as ApplicationUserManager  ;
//        //}

//        public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long,
//            ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
//        {
//            public ApplicationDbContext()
//                : base("OkrsContext")
//            {
//            }

//            public static ApplicationDbContext Create()
//            {
//                return new ApplicationDbContext();
//            }
//        }
//    }
//}
