using System.Configuration;
using CrashReport.Infrastructure.Nhibernate;
using CrashReport.Infrastructure.Repositories;
using CrashReportViewer.Infrastructure.RedmineIntegration;
using CrashReportViewer.Infrastructure.Services;
using Ninject.Web.Common;
using NLog;

namespace CrashReportViewer
{
	public class CoreModule: Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<ISessionFactoryProvider>().To<SessionFactoryProvider>().InSingletonScope()
				.WithConstructorArgument("connectionStringKey", "db")
				.OnDeactivation(x => x.Deactivate());
			Bind<ISessionProvider>().To<SessionProvider>().InRequestScope()
				.WithConstructorArgument("connectionStringKey", "db");

			Bind<IProblemStatisticsService>().To<ProblemStatisticsService>().InRequestScope();
			Bind<IProblemRepository>().To<ProblemRepository>().InRequestScope();
			Bind<ICrashRepository>().To<CrashRepository>().InRequestScope();

			Bind<Logger>().ToMethod(c => LogManager.GetCurrentClassLogger()).InSingletonScope();

			Bind<IRedmineIntegrationService>().To<RedmineIntegrationService>().InSingletonScope()
				.WithConstructorArgument("apiKey", ConfigurationManager.AppSettings["RedmineApiKey"])
				.WithConstructorArgument("redmineUrl", ConfigurationManager.AppSettings["RedmineUrl"]);
		}
	}
}