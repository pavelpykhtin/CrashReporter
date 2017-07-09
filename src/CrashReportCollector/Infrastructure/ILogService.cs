using CrashReport.Domain;

namespace CrashReportCollector.Infrastructure
{
	public interface ILogService
	{
		void LogMessage(string applicationKey, Message message);
	}
}