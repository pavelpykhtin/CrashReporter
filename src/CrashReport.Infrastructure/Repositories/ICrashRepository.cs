using System;
using System.Collections.Generic;
using CrashReport.Domain;

namespace CrashReport.Infrastructure.Repositories
{
	public interface ICrashRepository: IRepository<Crash>
	{
		IEnumerable<Crash> GetByProblemId(int problemId, int numberOfLast);
		IEnumerable<Crash> GetCrashesInRange(DateTime? lastInvokeTime, DateTime invokedOn);
		IEnumerable<Crash> GetLatestCrashes(int @from, int to);
	}
}