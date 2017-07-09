using NHibernate;

namespace CrashReport.Infrastructure.Nhibernate
{
	public interface ISessionFactoryProvider
	{
		ISessionFactory Factory { get; }
	}
}