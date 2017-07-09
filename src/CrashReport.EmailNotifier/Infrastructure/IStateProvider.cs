using System;

namespace CrashReport.EmailNotifier.Infrastructure
{
	public interface IStateProvider
	{
		DateTime? GetLastInvokeTime();
		void SetLastInvokeTime(DateTime invokedOn);
	}
}