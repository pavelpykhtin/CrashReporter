namespace CrashReportCollector.Infrastructure.JSStackTraceBeautifier
{
	public static class ConvertUtils
	{
		public static int? SafeToInt(this string src)
		{
			int res;
			if (!int.TryParse(src, out res))
				return null;

			return res;
		}
	}
}