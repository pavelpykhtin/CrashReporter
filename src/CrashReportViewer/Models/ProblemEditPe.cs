using CrashReport.Domain;

namespace CrashReportViewer.Models
{
	public class ProblemEditPe
	{
		public string shortDescription { get; set; }
		public string description { get; set; }
		public string fixedInVersion { get; set; }

		public EProblemStatus? status { get; set; }
	}
}