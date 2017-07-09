using System;
using NHibernate;

namespace CrashReport.Infrastructure.Nhibernate
{
	public class SimpleSessionProvider : ISessionProvider
	{
		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private bool _isSessionOpened;

		public ISession Session
		{
			get
			{
				lock (this)
				{
					if (InternalSession == null)
						throw new InvalidOperationException("There is no opened session");

					return InternalSession;
				}
			}
		}

		public bool IsSessionOpened
		{
			get { return _isSessionOpened; }
		}

		private ISession InternalSession { get; set; }

		public SimpleSessionProvider(ISessionFactoryProvider sessionFactoryProvider)
		{
			_sessionFactoryProvider = sessionFactoryProvider;
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