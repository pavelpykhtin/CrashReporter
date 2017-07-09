using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CrashReportCollector.Infrastructure.JSStackTraceBeautifier
{
	public class StackTraceBeautifier
	{
		public string Beautify(ISourceMapProvider sourceMapProvider, string src)
		{
			var sb = new StringBuilder();

			var rows = ParseStackTrace(src);

			rows = rows
				.Select(x => MapRow(sourceMapProvider, x));

			foreach (var row in rows)
			{
				sb.Append($"{row.File} [{row.Line}, {row.Column}]\r\n");
			}

			return sb.ToString();
		}

		private SourceRow MapRow(ISourceMapProvider sourceMapProvider, SourceRow src)
		{
			var sourceMap = sourceMapProvider.Get(src.File);

			if (sourceMap == null)
				return src;

			if (src.Line == null)
				return src;

			if (src.Column == null)
				return src;

			var position = sourceMap.OriginalPositionFor((int)src.Line, (int)src.Column);

			return new SourceRow
			{
				Line = position.Line,
				Column = position.Column,
				File = position.Source,
				Symbol = position.Name
			};
		}

		private IEnumerable<SourceRow> ParseStackTrace(string source)
		{
			var mozilaRegex = new Regex(@"(?:^[\w\W].*?@(?<f>[^\?\#]+)(?<p>[\w\W]*?):(?<l>\d+):(?<c>\d+)\r?$)+?", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
			var chromeRegex = new Regex(@"^(?:(?:.*?)\((?<f>[^\(\s\?\#]+)(?<p>[^\(]*?):(?<l>\d+):(?<c>\d+)\)|(?:\s+[^\s]*\s)(?:\[[^\]]+\])?(?<f>[^\(\s\?\#]+)(?<p>[^\(]*?):(?<l>\d+):(?<c>\d+))\r?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
			var regexes = new[]
			{
				mozilaRegex,
				chromeRegex
			};

			var regex = regexes.FirstOrDefault(x => x.IsMatch(source));

			if (regex == null)
				yield break;

			var matches = regex.Matches(source).Cast<Match>().ToArray();

			foreach (var match in matches)
			{
				yield return new SourceRow
				{
					Line = match.Groups["l"].Value.SafeToInt(),
					Column = match.Groups["c"].Value.SafeToInt(),
					File = match.Groups["f"].Value
				};
			}
		}
	}
}