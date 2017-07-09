using CrashReport.Domain;
using FluentNHibernate.Mapping;

namespace CrashReport.Infrastructure.Mappings
{
	public class ApplicationMap : ClassMap<Application>
	{
		public ApplicationMap()
		{
			Table("application");
			Id(x => x.Id);

			Map(x => x.Key).Column("[key]");
			Map(x => x.Name);
			Map(x => x.Language);
		}
	}
}