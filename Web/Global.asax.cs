using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.Repositories;
using Infrastructure.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using Web.Common;
using Web.Identity;
namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MapperConfig.RegisterMapping();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterDisposableTransient<IUnitOfWork, UnitOfWork>();
            container.RegisterDisposableTransient<IUserStore<IdentityUser, int>, UserStore>();
            container.Register<RoleStore>(() => new RoleStore(container.GetInstance<UnitOfWork>()), Lifestyle.Scoped);
            container.Register<UserManager<IdentityUser, int>>(() =>
            {
                var usermanager =
                    new UserManager<IdentityUser, int>(container.GetInstance<UserStore>())
                    {
                        SmsService = new SmsService(),
                        EmailService = new EmailService()
                    };

                usermanager.UserValidator = new UserValidator<IdentityUser, int>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                usermanager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonLetterOrDigit = true,
                    RequireUppercase = true,
                    RequiredLength = 8
                };

                usermanager.UserLockoutEnabledByDefault = true;
                usermanager.MaxFailedAccessAttemptsBeforeLockout = 5;
                usermanager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);


                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug it in here.
                usermanager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser, int>
                {
                    MessageFormat = "Your security code is {0}"
                });
                usermanager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser, int>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                });

                return usermanager;
            }, Lifestyle.Scoped);

            container.Register<SignInManager<IdentityUser, int>>(Lifestyle.Scoped);

            container.Register(() => container.IsVerifying ? new OwinContext().Authentication : HttpContext.Current.GetOwinContext().Authentication, Lifestyle.Scoped);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
