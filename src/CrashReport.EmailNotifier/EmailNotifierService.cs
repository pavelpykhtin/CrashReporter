using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Web;
using CrashReport.Domain;
using CrashReport.Infrastructure;
using CrashReport.Infrastructure.Nhibernate;
using NLog;

namespace CrashReport.EmailNotifier
{
	public partial class EmailNotifierService : ServiceBase
	{
		private readonly ISessionProvider _sessionProvider;
		private Timer _timer;
		private bool _processing;
		private readonly Settings _settings;
		private readonly Logger _logger;
		private readonly IEmailNotificationRepository _emailNotificationRepository;

		public EmailNotifierService(
			ISessionProvider sessionProvider, 
			Settings settings,
			Logger logger, 
			IEmailNotificationRepository emailNotificationRepository)
		{
			_sessionProvider = sessionProvider;
			_settings = settings;
			_logger = logger;
			_emailNotificationRepository = emailNotificationRepository;
			InitializeComponent();
			_processing = false;
		}

		protected override void OnStart(string[] args)
		{
			_logger.Info("Started");
			_timer = new Timer(CheckEmails, null, 0, 60000);
		}

		protected override void OnStop()
		{
			_logger.Info("Stopping");
			_timer.Dispose();
			_timer = null;
			_logger.Info("Stopped");
		}

		private void CheckEmails(object state)
		{
			_logger.Info("Checking for notifications");
			try
			{
				lock (this)
				{
					if (_processing)
					{
						_logger.Info("Throtling");

						return;
					}

					_processing = true;
				}

				_logger.Trace("Opening session");
				//Open session
				_sessionProvider.OpenSession();
				_logger.Trace("Opened session");

				//Get errors
				var pendingNotifications = _emailNotificationRepository.GetPending();

				//Process emails
				SendNotifications(pendingNotifications);

				_logger.Trace("Commiting");

				//Close session
				_sessionProvider.CloseSession();

				_logger.Trace("Commited");
			}
			catch (Exception e)
			{
				_logger.Fatal(e);
				throw;
			}
			finally
			{
				lock (this)
				{
					_processing = false;
				}
			}
		}

		private void SendNotifications(IEnumerable<EmailNotification> pendingNotifications)
		{
			if (!pendingNotifications.Any())
				return;

			using (var smtpClient = new SmtpClient(_settings.Host, _settings.Port))
			{
				smtpClient.EnableSsl = true;

				foreach (var notification in pendingNotifications)
				{
					smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

					var message = new MailMessage
					{
						From = _settings.From.ToMailAddress(),
						Subject = notification.Subject,
						Body = notification.Message,
						IsBodyHtml = true
					};

					message.To.Add(notification.Recepients);

					smtpClient.Send(message);

					notification.IsPending = false;

					_emailNotificationRepository.Save(notification);
				}
			}
		}
	}
}
