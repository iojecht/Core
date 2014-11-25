using System.Web;
using Core.Infrastructure;
using Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Core.DependencyResolution
{
    public class IdentityRegistry : Registry
    {
        public IdentityRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            For<ApplicationUserManager>().Use<ApplicationUserManager>(() => new ApplicationUserManager(new UserStore<ApplicationUser>()));

            For<ApplicationSignInManager>().Use<ApplicationSignInManager>(() => new ApplicationSignInManager(new ApplicationUserManager(new UserStore<ApplicationUser>()), HttpContext.Current.GetOwinContext().Authentication));

            For<ApplicationDbContext>().Use<ApplicationDbContext>(() => new ApplicationDbContext());

            For<IAuthenticationManager>().Use(c => HttpContext.Current.GetOwinContext().Authentication);

            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>().Ctor<ApplicationDbContext>();
        }
    }
}