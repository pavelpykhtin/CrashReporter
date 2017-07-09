using System;

namespace CrashReport.Domain
{
	public class Crash: DomainEntity
	{
		public virtual DateTime Timestamp { get; set; }
		public virtual ELogLevel LogLevel { get; set; }
		public virtual string Message { get; set; }
		public virtual string StackTrace { get; set; }
		public virtual string AdditionalInformation { get; set; }
		public virtual string InnerException { get; set; }
		public virtual string Version { get; set; }

		public virtual Problem Problem { get; set; }
		public virtual Application Application { get; set; }
	}
}