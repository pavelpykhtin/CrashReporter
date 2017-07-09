using System.Collections.Generic;
using CrashReport.Domain;
using CrashReport.Infrastructure.Repositories;

namespace CrashReport.Infrastructure
{
	public interface IEmailNotificationRepository: IRepository<EmailNotification>
	{
		IEnumerable<EmailNotification> GetPending();
	}
}