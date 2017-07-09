using CrashReport.Domain;

namespace CrashReportCollector.Infrastructure
{
	public interface ICrashProcessor
	{
		Crash Process(Crash crash);
	}
}