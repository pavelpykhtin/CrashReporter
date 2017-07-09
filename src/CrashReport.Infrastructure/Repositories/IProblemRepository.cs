using System.Collections.Generic;
using CrashReport.Domain;

namespace CrashReport.Infrastructure.Repositories
{
	public interface IProblemRepository: IRepository<Problem>
	{
		IEnumerable<Crash> GetLatestProblems(int from, int to);
		IEnumerable<Problem> GetFixedProblems();
		Problem GetByErrorHash(int errorHash);
		IEnumerable<Problem> GetRelatedProblems(int problemId);
	}
}