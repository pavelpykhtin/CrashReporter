using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CrashReport.Infrastructure.Nhibernate;
using CrashReportViewer.App_Start;
using NLog;

namespace CrashReportViewer
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			BeginRequest += application_OnBeginRequest;
			EndRequest += application_OnEndRequest;

			Error += application_OnError;

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private void application_OnError(object sender, EventArgs e)
		{
			var logger = DependencyResolver.Current.GetService<Logger>();

			logger.Fatal(Server.GetLastError());

			Server.ClearError();
		}

		private void application_OnEndRequest(object sender, EventArgs e)
		{
			DependencyResolver.Current.GetService<ISessionProvider>().CloseSession();
		}

		private void application_OnBeginRequest(object sender, EventArgs e)
		{
			DependencyResolver.Current.GetService<ISessionProvider>().OpenSession();
		}
	}
}