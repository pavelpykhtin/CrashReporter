using System;

namespace CrashReport.Domain
{
	public class EmailNotification : DomainEntity
	{
		public virtual DateTime CreatedAt { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Message { get; set; }
		public virtual string Recepients { get; set; }
		public virtual bool IsPending { get; set; }
	}
}