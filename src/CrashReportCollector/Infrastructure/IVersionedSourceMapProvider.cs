using CrashReportCollector.Infrastructure.JSStackTraceBeautifier;

namespace CrashReportCollector.Infrastructure
{
	public interface IVersionedSourceMapProvider
	{
		ISourceMapProvider GetByApplicationVersion(string application, string appVersion);
	}
}