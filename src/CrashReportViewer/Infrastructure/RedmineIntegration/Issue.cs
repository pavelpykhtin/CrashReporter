namespace CrashReportViewer.Infrastructure.RedmineIntegration
{
	public class Issue
	{
		public int? id { get; set; }
		public string subject { get; set; }
		public string description { get; set; }
		public int? assigned_to_id { get; set; }
		public int tracker_id { get; set; }
		public string project_id { get; set; }
	}
}