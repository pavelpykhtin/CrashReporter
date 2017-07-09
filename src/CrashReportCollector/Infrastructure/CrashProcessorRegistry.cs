using System.Collections.Generic;
using System.Linq;
using CrashReport.Domain;

namespace CrashReportCollector.Infrastructure
{
	public class CrashProcessorRegistry : ICrashProcessorRegistry
	{
		private readonly List<CrashProcessorInfo> _innerStorage;

		public CrashProcessorRegistry()
		{
			_innerStorage = new List<CrashProcessorInfo>();
		}

		public IEnumerable<ICrashProcessor> Get(ELogLevel logLevel, string language)
		{
			return _innerStorage
				.Where(x => x.Language == null || x.Language == language)
				.Where(x => x.LogLevel == null || x.LogLevel == logLevel)
				.Select(x => x.Instance);
		}

		public void Register(ICrashProcessor instance, string language, ELogLevel logLevel)
		{
			_innerStorage.Add(new CrashProcessorInfo
			{
				Instance = instance,
				Language = language,
				LogLevel = logLevel
			});
		}


		private class CrashProcessorInfo
		{
			public string Language { get; set; }
			public ELogLevel? LogLevel { get; set; }
			public ICrashProcessor Instance { get; set; }
		}
	}
}