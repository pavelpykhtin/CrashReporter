using System.Collections.Generic;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Nhibernate;
using NHibernate.Linq;

namespace CrashReport.Infrastructure
{
	public class EmailNotificationRepository : RepositoryBase<EmailNotification>, IEmailNotificationRepository
	{
		public EmailNotificationRepository(ISessionProvider sessionProvider) : base(sessionProvider)
		{

		}

		public IEnumerable<EmailNotification> GetPending()
		{
			return _sessionProvider.Session.Query<EmailNotification>()
				.Where(x => x.IsPending)
				.ToArray();
		}
	}
}