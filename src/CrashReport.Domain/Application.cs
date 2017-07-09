namespace CrashReport.Domain
{
	public class Application: DomainEntity
	{
		public virtual string Name { get; set; }
		public virtual string Key { get; set; }
		public virtual string Language { get; set; }
	}
}