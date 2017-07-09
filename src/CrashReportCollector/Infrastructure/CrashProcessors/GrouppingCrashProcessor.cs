using System;
using CrashReport.Domain;
using CrashReport.Infrastructure.Repositories;

namespace CrashReportCollector.Infrastructure.CrashProcessors
{
	public class GrouppingCrashProcessor: ICrashProcessor
	{
		private readonly IProblemRepository _problemRepository;

		public GrouppingCrashProcessor(IProblemRepository problemRepository)
		{
			_problemRepository = problemRepository;
		}

		public Crash Process(Crash crash)
		{
			crash.Problem = IdentifyProblem(crash);

			return crash;
		}

		private Problem IdentifyProblem(Crash crash)
		{
			var errorHash = GetErrorHash(crash);

			var problem = _problemRepository.GetByErrorHash(errorHash);

			if (problem != null)
				return problem;

			problem = new Problem
			{
				UniqueDescription = errorHash,
				Application = crash.Application,
				Description = crash.StackTrace,
				ShortDescription = crash.StackTrace.Substring(0, Math.Min(255, crash.StackTrace.Length)),
				Status = EProblemStatus.New
			};

			return problem;
		}

		private int GetErrorHash(Crash crash)
		{
			var stackTrace = crash.StackTrace ?? "";

			var result = stackTrace.GetHashCode();

			return result;
		}
	}
}