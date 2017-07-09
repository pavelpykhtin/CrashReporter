using System;
using System.Collections.Generic;
using System.IO;
using CrashReportCollector.Infrastructure.JSStackTraceBeautifier;

namespace CrashReportCollector.Infrastructure
{
	public class VersionedSourceMapProvider : IVersionedSourceMapProvider
	{
		private readonly Dictionary<Tuple<string, string>, ISourceMapProvider> _innerStorage;
		private readonly string _folder;

		public VersionedSourceMapProvider(string folder)
		{
			_folder = folder;
			_innerStorage = new Dictionary<Tuple<string, string>, ISourceMapProvider>();
		}

		public ISourceMapProvider GetByApplicationVersion(string application, string appVersion)
		{
			var key = new Tuple<string, string>(application, appVersion);

			if (_innerStorage.ContainsKey(key))
				return _innerStorage[key];

			var subPath = Path.Combine(_folder, application, appVersion);
			var provider = new SourceMapProvider(subPath);

			_innerStorage.Add(key, provider);

			return provider;
		}
	}
}