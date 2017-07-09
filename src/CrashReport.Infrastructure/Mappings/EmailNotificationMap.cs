using CrashReport.Domain;
using FluentNHibernate.Mapping;

namespace CrashReport.Infrastructure.Mappings
{
	public class EmailNotificationMap : ClassMap<EmailNotification>
	{
		public EmailNotificationMap()
		{
			Table("emailNotification");
			Id(x => x.Id).Column("id");

			Map(x => x.Subject);
			Map(x => x.Message).CustomType("StringClob");
			Map(x => x.CreatedAt);
			Map(x => x.Recepients);
			Map(x => x.IsPending);
		}
	}
}