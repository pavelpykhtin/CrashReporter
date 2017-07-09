using System.Configuration;
using CrashReport.Infrastructure;
using CrashReport.Infrastructure.Nhibernate;
using Ninject.Modules;
using NLog;

namespace CrashReport.EmailNotifier
{
	public class CoreModule : NinjectModule
	{
		public override void Load()
		{
			Bind<EmailNotifierService>().ToSelf().InSingletonScope()
				.WithConstructorArgument("settings", new Settings
				{
					Host = ConfigurationManager.AppSettings["SmtpHost"],
					Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
					Username = ConfigurationManager.AppSettings["SmtpUser"],
					Password = ConfigurationManager.AppSettings["SmtpPassword"],
					From = ConfigurationManager.AppSettings["From"],
				}); ;

			Bind<ISessionFactoryProvider>().To<SessionFactoryProvider>().InSingletonScope()
				.WithConstructorArgument("connectionStringKey", "db");
			Bind<ISessionProvider>().To<SimpleSessionProvider>().InSingletonScope();

			Bind<IEmailNotificationRepository>().To<EmailNotificationRepository>().InSingletonScope();

			Bind<Logger>().ToMethod(c => LogManager.GetCurrentClassLogger()).InSingletonScope();
		}
	}
}