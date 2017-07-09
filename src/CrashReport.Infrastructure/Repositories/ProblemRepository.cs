using System.Collections.Generic;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Nhibernate;
using NHibernate.Linq;

namespace CrashReport.Infrastructure.Repositories
{
	public class ProblemRepository: RepositoryBase<Problem>, IProblemRepository
	{
		public ProblemRepository(ISessionProvider sessionProvider) : base(sessionProvider)
		{

		}

		public IEnumerable<Crash> GetLatestProblems(int from, int to)
		{
			var latestCrashIds = _sessionProvider.Session.CreateSQLQuery(@"
				;with orderedErrorTypes as (
					select
						TypeId,
						Max(id) id
					from crash
					group by TypeId
				),
				core as (
					select 
						e.id,
						row_number() over (order by e.timestamp desc) idx
					from orderedErrorTypes
					inner join crash e on e.id = orderedErrorTypes.id and e.TypeId = orderedErrorTypes.TypeID
				)

				select 
					id 
				from core 
				where 
					idx between :from and :to
				order by idx")
				.SetParameter("from", from)
				.SetParameter("to", to)
				.List<int>();

			return _sessionProvider.Session.Query<Crash>()
				.Fetch(x => x.Problem).ThenFetch(x => x.Application)
				.Where(x => latestCrashIds.Contains(x.Id))
				.OrderByDescending(x => x.Timestamp)
				.ToList();
		}

		public IEnumerable<Problem> GetFixedProblems()
		{
			return _sessionProvider.Session.Query<Problem>()
				.Where(x => x.Status == EProblemStatus.Fixed)
				.ToList();
		}

		public Problem GetByErrorHash(int errorHash)
		{
			return _sessionProvider.Session.Query<Problem>()
				.Where(x => x.UniqueDescription == errorHash)
				.FirstOrDefault();
		}

		public IEnumerable<Problem> GetRelatedProblems(int problemId)
		{
			var relatedProblems = _sessionProvider.Session.CreateSQLQuery(@"
				; with relatedCrashes as (
					 select
						me.typeId problemId,
						related.typeId relatedProblemId
					from crash me
					inner
					join crash related on abs(datediff(Second, me.Timestamp, related.Timestamp)) < 5 and me.typeId != related.typeId)

				select problem.id
				from problem
				inner join relatedCrashes crashes on crashes.relatedProblemId = problem.id
				where
					crashes.problemId = :problemId")
				.SetParameter("problemId", problemId)
				.List<int>();

			return _sessionProvider.Session.Query<Problem>()
				.Where(x => relatedProblems.Contains(x.Id))
				.ToList();
		}
	}
}