using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Linq;
using System.Linq;
using CrashReport.Domain;
using CrashReport.Infrastructure.Nhibernate;

namespace CrashReportViewer.Infrastructure.Services
{
	public class ProblemStatisticsService : IProblemStatisticsService
	{
		private readonly ISessionProvider _sessionProvider;

		public ProblemStatisticsService(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public IEnumerable<ProblemStatistics> ForPeriod(DateTime from, DateTime to)
		{
			return _sessionProvider.Session.CreateCriteria<Crash>()
				.Add(Restrictions.Eq("LogLevel", ELogLevel.Fatal))
				.Add(Restrictions.Gt("Timestamp", from))
				.Add(Restrictions.Lt("Timestamp", to))
				.SetProjection(
					Projections.ProjectionList()
						.Add(Projections.GroupProperty("Problem"), "Problem")
						.Add(Projections.RowCount(), "Occurences"))
				.SetResultTransformer(Transformers.AliasToBean<ProblemStatistics>())
				.List<ProblemStatistics>();
		}

		public IEnumerable<Crash> LatestCrashes(int amount)
		{
			return _sessionProvider.Session.Query<Crash>()
				.Where(x => x.LogLevel == ELogLevel.Fatal)
				.OrderByDescending(x => x.Timestamp)
				.Take(amount)
				.ToList();
		}

		public IEnumerable<StatisticsPoint> CrashesPerWeek()
		{
			return _sessionProvider.Session.CreateSQLQuery(@"
					;with errors as (
						select 
							datediff(day, Timestamp, GetDate()) / 1 dateKey,
							count(*) Mark,
							0 isFake
						from crash e
						where 
							e.LogLevel = 'Fatal' and 
							Timestamp > '2015-10-01'
						group by 
							datediff(day, Timestamp, GetDate()) / 1
					),
					extendedErrors as (
						select * from errors
						union all
						select 0 dateKey, 0 Mark, 1 isFake
					),
					alignedErrors as (
						select 
							*,
							row_number() over (partition by dateKey order by isFake) idx
						from extendedErrors
					)					

					select 
						-dateKey Date,
						alignedErrors.Mark Value
					from alignedErrors
					where alignedErrors.idx = 1
					order by alignedErrors.dateKey desc
				")
				.SetResultTransformer(Transformers.AliasToBean(typeof(StatisticsPoint)))
				.List<StatisticsPoint>();
		}

		public IEnumerable<StatisticsPoint> SignupsPerWeek()
		{
			return Enumerable.Empty<StatisticsPoint>();
			return _sessionProvider.Session.CreateSQLQuery(@"
					with newUsers as (
						select 
							datediff(day, ValidFrom, GetDate()) / 7 monthKey,
							count(*) Mark
						from tUser u
						where ValidFrom > '2015-10-01'
						group by 
							datediff(day, ValidFrom, GetDate()) / 7
					)

					select 
						Mark Value,
						-monthKey Date
					from newUsers
					order by newUsers.monthKey desc
					")
				.SetResultTransformer(Transformers.AliasToBean(typeof(StatisticsPoint)))
				.List<StatisticsPoint>();
		}

		public IEnumerable<StatisticsPoint> ErrorsPerUserPerWeek()
		{
			return Enumerable.Empty<StatisticsPoint>();

			return _sessionProvider.Session.CreateSQLQuery(@"
					with newUsers as (
						select 
							datediff(day, ValidFrom, GetDate()) / 7 monthKey,
							count(*) Mark
						from tUser u
						where ValidFrom > '2015-10-01'
						group by 
							datediff(day, ValidFrom, GetDate()) / 7
					),
					errors as (
						select 
							datediff(day, Timestamp, GetDate()) / 7 monthKey,
							count(*) Mark
						from crash e
						where 
							e.LogLevel = 'Fatal' and 
							Timestamp > '2015-10-01'
							--Module != 'WebWeaver.Client'
						group by 
							datediff(day, Timestamp, GetDate()) / 7
					)

					select 
						100 * e.Mark / u.Mark Value,
						-isnull(u.monthKey, e.monthKey) Date
					from newUsers u
					full join errors e on e.monthKey = u.monthKey
					order by isnull(u.monthKey, e.monthKey) desc
					")
				.SetResultTransformer(Transformers.AliasToBean(typeof(StatisticsPoint)))
				.List<StatisticsPoint>();
		}
	}
}