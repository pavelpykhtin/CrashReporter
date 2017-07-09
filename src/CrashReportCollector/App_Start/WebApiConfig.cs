using System.Web.Http;

namespace CrashReportCollector
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "AppActions",
				routeTemplate: "api/{applicationKey}/{controller}"
			);

			config.EnableCors();
		}
	}
}
