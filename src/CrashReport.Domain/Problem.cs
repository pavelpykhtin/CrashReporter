using System.Collections.Generic;

namespace CrashReport.Domain
{
	public class Problem: DomainEntity
	{
		public virtual string Module { get; set; }
		public virtual string ShortDescription { get; set; }
		public virtual string Description { get; set; }
		public virtual int UniqueDescription { get; set; }
		public virtual bool Cleared { get; set; }
		public virtual string FixedInVersion { get; set; }
		public virtual EProblemStatus? Status { get; set; }
		public virtual ICollection<Crash> Crashes { get; set; }
		public virtual Application Application { get; set; }
		public virtual string ExternalId { get; set; }

		public Problem()
		{
			Crashes = new List<Crash>();
		}
	}
}