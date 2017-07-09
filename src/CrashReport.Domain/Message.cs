using System;

namespace CrashReport.Domain
{
	public class Message
	{
		public DateTime? TimeStamp { get; set; }
		public ELogLevel LogLevel { get; set; }
		public string MessageText { get; set; }
		public string StackTrace { get; set; }
		public string AdditionalInformation { get; set; }
		public string InnerException { get; set; }
		public string Version { get; set; }
	}
}