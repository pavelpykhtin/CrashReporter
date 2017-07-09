using CrashReport.Infrastructure.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace CrashReport.Infrastructure.Nhibernate
{
	public class SessionFactoryProvider: ISessionFactoryProvider
	{
		private ISessionFactory _sessionFactory;

		public ISessionFactory Factory
		{
			get { return _sessionFactory; }
		}

		public SessionFactoryProvider(string connectionStringKey)
		{
			var cfg = Fluently.Configure()
				.Database(
					MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey(connectionStringKey)).ShowSql())
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Marker>())
				.ExposeConfiguration(c =>
				{
					
				})
				.CurrentSessionContext("call")
				.BuildConfiguration();

			_sessionFactory = cfg.BuildSessionFactory();
		}

		public void Deactivate()
		{
			if (_sessionFactory != null) _sessionFactory.Close();
			_sessionFactory = null;
		}
	}
}