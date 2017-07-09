using System;
using System.Collections.Generic;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Nhibernate;
using NHibernate.Linq;

namespace CrashReport.Infrastructure.Repositories
{
	public class CrashRepository: RepositoryBase<Crash>, ICrashRepository
	{
		public CrashRepository(ISessionProvider sessionProvider) : base(sessionProvider)
		{

		}

		public IEnumerable<Crash> GetByProblemId(int problemId, int numberOfLast)
		{
			return _sessionProvider.Session.Query<Crash>()
				.Where(x => x.Problem.Id == problemId)
				.OrderByDescending(x => x.Timestamp)
				.Take(numberOfLast)
				.ToList();
		}

		public IEnumerable<Crash> GetCrashesInRange(DateTime? lastInvokeTime, DateTime invokedOn)
		{
			return _sessionProvider.Session.Query<Crash>()
				.Where(x => x.LogLevel == ELogLevel.Fatal)
				.Where(x => (lastInvokeTime == null || x.Timestamp > lastInvokeTime) && x.Timestamp <= invokedOn)
				.OrderByDescending(x => x.Timestamp)
				.ToList();
		}

		public IEnumerable<Crash> GetLatestCrashes(int @from, int to)
		{
			return _sessionProvider.Session.Query<Crash>()
				.Where(x => x.LogLevel == ELogLevel.Fatal)
				.OrderByDescending(x => x.Timestamp)
				.Skip(@from - 1)
				.Take(to - @from + 1)
				.ToList();
		}
	}
}