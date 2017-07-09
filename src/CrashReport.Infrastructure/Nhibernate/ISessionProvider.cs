using NHibernate;

namespace CrashReport.Infrastructure.Nhibernate
{
	public interface ISessionProvider
	{
		ISession Session { get; }
		bool IsSessionOpened { get; }
		void OpenSession();
		void CloseSession();
	}
}