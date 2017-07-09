using System.Configuration;
using CrashReportCollector.Infrastructure;
using CrashReportCollector.Infrastructure.CrashProcessors;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CrashReportCollector.NInject
{
	public class CrashProcessingModule: NinjectModule
	{
		public override void Load()
		{
			Bind<ICrashProcessor>().To<GrouppingCrashProcessor>().InRequestScope().Named(Configuration.CrashProcessors.Groupping);

			Bind<IVersionedSourceMapProvider>().To<VersionedSourceMapProvider>().InSingletonScope()
				.WithConstructorArgument("folder", ConfigurationManager.AppSettings.Get("MapFolder"));
			Bind<ICrashProcessor>().To<JSStackTraceBeautifyCrashProcessor>().InRequestScope().Named(Configuration.CrashProcessors.Beautify);
			Bind<ICrashProcessor>().To<RepeatedIssueCrashProcessor>().InRequestScope().Named(Configuration.CrashProcessors.RepeatedIssue);
			Bind<ICrashProcessor>().To<EmailNotificationCrashProcessor>().InRequestScope().Named(Configuration.CrashProcessors.EmailNotification)
				.WithConstructorArgument("recepients", ConfigurationManager.AppSettings.Get("EmailNotificationRecepients"));
		}
	}
}