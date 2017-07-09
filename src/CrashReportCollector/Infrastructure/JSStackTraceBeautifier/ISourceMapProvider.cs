using SourceMap.Net;

namespace CrashReportCollector.Infrastructure.JSStackTraceBeautifier
{
	public interface ISourceMapProvider
	{
		ISourceMapConsumer Get(string minifiedFileName);
	}
}