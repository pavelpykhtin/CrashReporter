using CrashReport.Domain;
using CrashReportCollector.Infrastructure.JSStackTraceBeautifier;

namespace CrashReportCollector.Infrastructure.CrashProcessors
{
	public class JSStackTraceBeautifyCrashProcessor : ICrashProcessor
	{
		private readonly StackTraceBeautifier _stackTraceBeautifier;
		private readonly IVersionedSourceMapProvider _sourceMapProvider;

		public JSStackTraceBeautifyCrashProcessor(
			StackTraceBeautifier stackTraceBeautifier, 
			IVersionedSourceMapProvider sourceMapProvider)
		{
			_stackTraceBeautifier = stackTraceBeautifier;
			_sourceMapProvider = sourceMapProvider;
		}

		public Crash Process(Crash crash)
		{
			var appVersion = crash.Version;

			ISourceMapProvider providerOnVersion = _sourceMapProvider.GetByApplicationVersion(crash.Application.Key, appVersion);

			crash.StackTrace = _stackTraceBeautifier.Beautify(providerOnVersion, crash.StackTrace);

			return crash;
		}
	}
}