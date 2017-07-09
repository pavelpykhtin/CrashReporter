using CrashReport.Domain;
using FluentNHibernate.Mapping;

namespace CrashReport.Infrastructure.Mappings
{
	public class CrashMap : ClassMap<Crash>
	{
		public CrashMap()
		{
			Table("crash");
			Id(x => x.Id).Column("id");

			Map(x => x.LogLevel).CustomType<GenericEnumMapper<ELogLevel>>();
			Map(x => x.Message).CustomType("StringClob");
			Map(x => x.StackTrace).CustomType("StringClob");
			Map(x => x.Timestamp);
			Map(x => x.AdditionalInformation).CustomType("StringClob");
			Map(x => x.InnerException).CustomType("StringClob");
			Map(x => x.Version);

			References(x => x.Problem).Column("typeId").Cascade.All();
			References(x => x.Application).Column("applicationId");
		}
	}
}