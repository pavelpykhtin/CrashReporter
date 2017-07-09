using CrashReport.Domain;

namespace CrashReportViewer.Infrastructure.Services
{
	public class ProblemStatistics
	{
		public Problem Problem { get; set; }
		public int Occurences { get; set; }
	}
}