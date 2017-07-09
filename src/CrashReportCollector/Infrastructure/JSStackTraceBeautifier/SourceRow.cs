namespace CrashReportCollector.Infrastructure.JSStackTraceBeautifier
{
	public class SourceRow
	{
		public int? Line { get; set; }
		public int? Column { get; set; }
		public string File { get; set; }
		public string Symbol { get; set; }
	}
}