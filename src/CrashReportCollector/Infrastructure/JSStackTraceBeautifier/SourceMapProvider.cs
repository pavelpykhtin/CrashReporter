using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SourceMap.Net;

namespace CrashReportCollector.Infrastructure.JSStackTraceBeautifier
{
	public class SourceMapProvider: ISourceMapProvider
	{
		private readonly Regex _fileNameRegex;
		private readonly Dictionary<string, ISourceMapConsumer> _sourceMaps;
		private readonly string _sourceMapFolder;
		private Dictionary<string, string> _maps;
		
		public SourceMapProvider(string sourceMapFolder)
		{
			_sourceMapFolder = sourceMapFolder;
			_fileNameRegex = new Regex(@"\A(?:.*?)/?(?<file>[^\/\#\?]+)(\#.*|\?.*)?\Z");
			_sourceMaps = new Dictionary<string, ISourceMapConsumer>();
			_maps = new Dictionary<string, string>();
		}

		public ISourceMapConsumer Get(string minifiedFileName)
		{
			var sourceMapKey = GetSourceMapKey(minifiedFileName);

			if (_sourceMaps.ContainsKey(sourceMapKey))
				return _sourceMaps[sourceMapKey];

			var sourceMap = CreateSourceMap(minifiedFileName);

			_sourceMaps.Add(sourceMapKey, sourceMap);

			return _sourceMaps[sourceMapKey];
		}

		private ISourceMapConsumer CreateSourceMap(string file)
		{
			var mapFile = GetMapFile(file);

			if (mapFile == null)
				return null;

			return SourceMapConsumer.GetConsumer(mapFile);
		}

		private string GetSourceMapKey(string file)
		{
			file = _fileNameRegex.Match(file).Groups["file"].Value;

			return file.ToLower().Trim();
		}

		private string GetMapFile(string file)
		{
			file = _fileNameRegex.Match(file).Groups["file"].Value;

			var sourceMapPath = Path.Combine(_sourceMapFolder, $"{file.ToLower()}.map");

			if (!File.Exists(sourceMapPath))
				return null;

			return File.ReadAllText(sourceMapPath);
		}
	}
}