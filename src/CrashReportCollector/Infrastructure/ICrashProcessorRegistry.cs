using System.Collections.Generic;
using CrashReport.Domain;

namespace CrashReportCollector.Infrastructure
{
	public interface ICrashProcessorRegistry
	{
		IEnumerable<ICrashProcessor> Get(ELogLevel logLevel, string language);
		void Register(ICrashProcessor instance, string language, ELogLevel logLevel);
	}
}