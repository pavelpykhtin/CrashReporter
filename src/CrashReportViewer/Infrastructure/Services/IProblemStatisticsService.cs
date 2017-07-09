using System;
using System.Collections.Generic;
using CrashReport.Domain;

namespace CrashReportViewer.Infrastructure.Services
{
	public interface IProblemStatisticsService
	{
		IEnumerable<ProblemStatistics> ForPeriod(DateTime from, DateTime to);
		IEnumerable<Crash> LatestCrashes(int amount);
		IEnumerable<StatisticsPoint> CrashesPerWeek();
		IEnumerable<StatisticsPoint> SignupsPerWeek();
		IEnumerable<StatisticsPoint> ErrorsPerUserPerWeek();
	}

	public class StatisticsPoint
	{
		public int Value { get; set; }
		public int Date { get; set; }
	}
}