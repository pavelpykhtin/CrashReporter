using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Nhibernate;
using NHibernate.Linq;

namespace CrashReport.Infrastructure.Repositories
{
	public interface IApplicationRepository : IRepository<Application>
	{
		Application GetByKey(string key);
	}

	public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
	{
		public ApplicationRepository(ISessionProvider sessionProvider) : base(sessionProvider)
		{
		}

		public Application GetByKey(string key)
		{
			return _sessionProvider.Session.Query<Application>()
				.Where(x => x.Key == key)
				.FirstOrDefault();
		}
	}
}