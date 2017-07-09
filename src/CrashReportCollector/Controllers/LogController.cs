using System.Web.Http;
using System.Web.Http.Cors;
using CrashReport.Domain;
using CrashReportCollector.Infrastructure;

namespace CrashReportCollector.Controllers
{
	[EnableCors("*", "*", "*")]
	public class LogController : ApiController
	{
		private readonly ILogService _logService;
		
		public LogController(ILogService logService)
		{
			_logService = logService;
		}

		public void Post(
			[FromBody] Message message,
			[FromUri] string applicationKey)
		{
			_logService.LogMessage(applicationKey, message);
		}
	}
}
