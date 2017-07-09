using System;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Repositories;

namespace CrashReportCollector.Infrastructure
{
	public class LogService : ILogService
	{
		private readonly ICrashRepository _crashRepository;
		private readonly IApplicationRepository _applicationRepository;
		private readonly ICrashProcessorRegistry _crashProcessorsRegistry;

		public LogService(
			ICrashRepository crashRepository, 
			IApplicationRepository applicationRepository, 
			ICrashProcessorRegistry crashProcessorsRegistry)
		{
			_crashRepository = crashRepository;
			_applicationRepository = applicationRepository;
			_crashProcessorsRegistry = crashProcessorsRegistry;
		}

		public void LogMessage(string applicationKey, Message message)
		{
			var application = _applicationRepository.GetByKey(applicationKey);

			if (application == null)
				return;

			var error = new Crash
			{
				AdditionalInformation = message.AdditionalInformation,
				InnerException = message.InnerException,
				LogLevel = message.LogLevel,
				Message = message.MessageText,
				StackTrace = message.StackTrace,
				Timestamp = DateTime.Now,
				Version = message.Version,
				Application = application
			};

			error = RunHandlers(error);

			_crashRepository.Save(error);
		}

		private Crash RunHandlers(Crash error)
		{
			var handlers = _crashProcessorsRegistry.Get(error.LogLevel, error.Application.Language);

			return handlers.Aggregate(error, (current, handler) => handler.Process(current));
		}
	}
}