using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using WebApiRestServiceExample.Controllers;
using WebApiRestServiceExample.Providers;

namespace WebApiRestServiceExample
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IGoogleMapsProvider, GoogleMapsProvider>();
            container.RegisterType<IDateTimeProvider, DateTimeProvider>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}