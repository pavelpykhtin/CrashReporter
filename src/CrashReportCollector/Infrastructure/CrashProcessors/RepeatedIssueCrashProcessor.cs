using CrashReport.Domain;

namespace CrashReportCollector.Infrastructure.CrashProcessors
{
	public class RepeatedIssueCrashProcessor : ICrashProcessor
	{
		public Crash Process(Crash crash)
		{
			var problem = crash.Problem;

			if (problem.Status != EProblemStatus.Deployed)
				return crash;

			crash.Problem.Status = EProblemStatus.Repeated;

			return crash;
		}

	}
}