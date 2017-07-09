using CrashReport.Domain;
using CrashReport.Infrastructure;
using CrashReport.Infrastructure.Nhibernate;
using CrashReport.Infrastructure.Repositories;
using CrashReportCollector.Infrastructure;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CrashReportCollector.NInject
{
	public class CoreModule: NinjectModule
	{
		public override void Load()
		{
			Bind<ISessionFactoryProvider>().To<SessionFactoryProvider>().InRequestScope()
				.WithConstructorArgument("connectionStringKey", "db");
			Bind<ISessionProvider>().To<SessionProvider>().InRequestScope()
				.WithConstructorArgument("connectionStringKey", "db");

			Bind<ICrashRepository>().To<CrashRepository>().InRequestScope();
			Bind<IProblemRepository>().To<ProblemRepository>().InRequestScope();
			Bind<IApplicationRepository>().To<ApplicationRepository>().InRequestScope();
			Bind<IEmailNotificationRepository>().To<EmailNotificationRepository>().InRequestScope();

			Bind<ILogService>().To<LogService>().InRequestScope();
			Bind<ICrashProcessorRegistry>().To<CrashProcessorRegistry>().InRequestScope()
				.OnActivation((c, r) =>
				{
					r.Register(c.Kernel.Get<ICrashProcessor>(Configuration.CrashProcessors.Beautify), "JS", ELogLevel.Fatal);
					r.Register(c.Kernel.Get<ICrashProcessor>(Configuration.CrashProcessors.Groupping), null, ELogLevel.Fatal);
					r.Register(c.Kernel.Get<ICrashProcessor>(Configuration.CrashProcessors.RepeatedIssue), null, ELogLevel.Fatal);
					r.Register(c.Kernel.Get<ICrashProcessor>(Configuration.CrashProcessors.EmailNotification), null, ELogLevel.Fatal);
				});
		}
	}
}