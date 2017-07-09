using CrashReport.Domain;

namespace CrashReportViewer.Infrastructure.RedmineIntegration
{
	public interface IRedmineIntegrationService
	{
		string CreateIssue(Problem problem, string projectId, string problemUrl);
	}
}