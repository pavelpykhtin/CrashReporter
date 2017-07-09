using CrashReport.Domain;
using FluentNHibernate.Mapping;

namespace CrashReport.Infrastructure.Mappings
{
	public class ProblemMap : ClassMap<Problem>
	{
		public ProblemMap()
		{
			Table("problem");
			Id(x => x.Id).Column("id");

			Map(x => x.Description).CustomType("StringClob");
			Map(x => x.ShortDescription);
			Map(x => x.UniqueDescription);
			Map(x => x.FixedInVersion);
			Map(x => x.ExternalId);
			Map(x => x.Status).CustomType<EProblemStatus>();
			Map(x => x.Cleared);

			HasMany(x => x.Crashes).KeyColumn("typeId").Inverse();
			References(x => x.Application).Column("applicationId");
		}
	}
}