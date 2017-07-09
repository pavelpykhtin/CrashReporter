using System.Web.Mvc;
using System.Web.Routing;

namespace CrashReportViewer.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "List.Short",
				url: "{controller}/list/{from}",
				defaults: new { action = "list", from = UrlParameter.Optional },
				constraints: new {from = "^[0-9]+$"}
			);

			routes.MapRoute(
				name: "List.Full",
				url: "{controller}/list/{from}-{to}",
				defaults: new { action = "list" },
				constraints: new { from = "^[0-9]+$", to = "^[0-9]+$" }
			);


			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}