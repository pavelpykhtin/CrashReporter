using System;

namespace CrashReportViewer.Utils
{
	public static class Utils
	{
		public static string FixLineBreaks(this string str)
		{
			if (str == null)
				return null;

			return str.Replace("\r\n", "<br>");
		}

		public static string TruncateTo(this string str, int length)
		{
			if (str == null)
				return null;

			return str.Substring(0, Math.Min(50, str.Length));
		}
	}
}