using System;
using System.Web;
using NHibernate;

namespace CrashReport.Infrastructure.Nhibernate
{
	public class SessionProvider : ISessionProvider
	{
		private readonly string _sessionStorageKey;
		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private bool _isSessionOpened;

		public ISession Session
		{
			get
			{
				lock (this)
				{
					if (InternalSession == null) throw new InvalidOperationException("There is no opened session");

					return InternalSession;
				}
			}
		}

		public bool IsSessionOpened
		{
			get { return _isSessionOpened; }
		}

		private ISession InternalSession
		{
			get { return (ISession) HttpContext.Current.Items[_sessionStorageKey]; }
			set { HttpContext.Current.Items[_sessionStorageKey] = value; }
		}

		public SessionProvider(string connectionStringKey, ISessionFactoryProvider sessionFactoryProvider)
		{
			_sessionFactoryProvider = sessionFactoryProvider;
			_sessionStorageKey = $"nhibernate_session_{connectionStringKey}";
		}

		public void OpenSession()
		{
			lock (this)
			{
				if (InternalSession != null) 
					throw new InvalidOperationException("Session already opened");

				InternalSession = _sessionFactoryProvider.Factory.OpenSession();
				_isSessionOpened = true;
			}
		}
		public void CloseSession()
		{
			lock (this)
			{
				InternalSession.Flush();
				InternalSession.Close();
				InternalSession.Dispose();

				InternalSession = null;
				_isSessionOpened = false;
			}
		}
	}
}
	