namespace CrashReportViewer.Infrastructure.RedmineIntegration
{
	public class IssueContainer
	{
		public Issue issue { get; set; }

		public IssueContainer(Issue content)
		{
			issue = content;
		}
	}
}