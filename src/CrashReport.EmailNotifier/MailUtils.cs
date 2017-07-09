using System.Collections.Generic;
using System.Net.Mail;

namespace CrashReport.EmailNotifier
{
	public static class MailUtils
	{
		public static MailAddress ToMailAddress(this string email)
		{
			return new MailAddress(email);
		}

		public static MailAddressCollection Add(this MailAddressCollection collection, IEnumerable<string> emails)
		{
			foreach (var email in emails)
				collection.Add(email.ToMailAddress());

			return collection;
		}
	}
}