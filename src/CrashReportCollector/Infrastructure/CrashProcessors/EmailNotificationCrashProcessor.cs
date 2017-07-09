using System;
using System.Web;
using CrashReport.Domain;
using CrashReport.Infrastructure;

namespace CrashReportCollector.Infrastructure.CrashProcessors
{
	public class EmailNotificationCrashProcessor: ICrashProcessor
	{
		private readonly string _recepients;
		private readonly IEmailNotificationRepository _emailNotificationRepository;

		public EmailNotificationCrashProcessor(string recepients, IEmailNotificationRepository emailNotificationRepository)
		{
			_recepients = recepients;
			_emailNotificationRepository = emailNotificationRepository;
		}

		public Crash Process(Crash crash)
		{
			PostNotification(crash);

			return crash;
		}

		private void PostNotification(Crash crash)
		{
			var emailNotification = new EmailNotification
			{
				CreatedAt = DateTime.Now,
				IsPending = true,
				Message = BuildMessageBody(crash),
				Subject = $"[CrashReport] New crash",
				Recepients = _recepients
			};

			_emailNotificationRepository.Save(emailNotification);
		}

		private string BuildMessageBody(Crash crash)
		{
			return $@"
		<html>
			<style type='text/css'>
				.value{{
					margin-left: 10px;
				}}
			</style>
			<div>
				<b>Module:</b>
				<div class='value'>
					{HttpUtility.HtmlEncode(crash.Application.Name).Replace("\r\n", "<br>")}
				</div>
			</div>

			<div>
				<b>Version:</b>
				<div class='value'>
					{HttpUtility.HtmlEncode(crash.Version).Replace("\r\n", "<br>")}
				</div>
			</div>

			<div>
				<b>Occured on:</b>
				<div class='value'>
					{HttpUtility.HtmlEncode(crash.Timestamp).Replace("\r\n", "<br>")}
				</div>
			</div>

			<div>
				<b>Message:</b>
				<div class='value'>
					{HttpUtility.HtmlEncode(crash.Message).Replace("\r\n", "<br>")}
				</div>
			</div>

			<div>
				<b>StackTrace:</b>
				<div class='value'>
					{HttpUtility.HtmlEncode(crash.StackTrace ?? string.Empty).Replace("\r\n", "<br>")}
				</div>
			</div>
		</html>

		";
		}
	}
}