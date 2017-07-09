using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CrashReport.Infrastructure.Nhibernate;

namespace CrashReportCollector
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		public override void Init()
		{
			base.Init();

			BeginRequest += OnBeginRequest;
			EndRequest += OnEndRequest;
		}

		private void OnEndRequest(object sender, EventArgs e)
		{
			var sessionProvider = (ISessionProvider)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof (ISessionProvider));

			sessionProvider.CloseSession();
		}

		private void OnBeginRequest(object sender, EventArgs e)
		{
			var sessionProvider = (ISessionProvider)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISessionProvider));

			sessionProvider.OpenSession();
		}
	}
}
