using System.ServiceProcess;
using Ninject;

namespace CrashReport.EmailNotifier
{
	static class Program
	{
		private static StandardKernel _kernel;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			Initialize();

			var ServicesToRun = new ServiceBase[]
			{
				_kernel.Get<EmailNotifierService>()
			};

			ServiceBase.Run(ServicesToRun);
		}

		private static void Initialize()
		{
			_kernel = new StandardKernel(
				new CoreModule());
		}
	}
}
