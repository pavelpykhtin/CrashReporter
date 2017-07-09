using System;
using System.Web.Mvc;
using CrashReportViewer.Infrastructure.Services;
using System.Linq;
using CrashReportViewer.Utils;

namespace CrashReportViewer.Controllers
{
	public class ProblemStatisticsController : Controller
	{
		private readonly IProblemStatisticsService _statisticsService;

		public ProblemStatisticsController(IProblemStatisticsService statisticsService)
		{
			_statisticsService = statisticsService;
		}

		public ActionResult LastWeek()
		{
			var statistics = _statisticsService.ForPeriod(DateTime.Now.AddDays(-7), DateTime.Now);

			return Json(statistics
					.Select(
						x => new
						     	{
						     		module = x.Problem?.Application?.Name,
						     		id = x.Problem?.Id.ToString(),
									hash = x.Problem?.UniqueDescription,
									status = x.Problem?.Status.ToString(),
									shortDescription = x.Problem?.ShortDescription?.TruncateTo(50),
									description = x.Problem?.Description,
						     		occurences = x.Occurences
						     	})
					.OrderByDescending(x => x.occurences)
					.ToArray());
		}

		public ActionResult LatestCrashes()
		{
			var crashes = _statisticsService.LatestCrashes(10);

			return Json(crashes.Select(
					x => new
					     	{
								id = x.Id,
								shortDescription = x.Problem.ShortDescription.TruncateTo(50),
								version = x.Version?.ToString(),
								date = x.Timestamp.ToString("dd.MM.yyyy HH:mm")
					     	}).ToArray());
		}

		public ActionResult CrashesPerWeek()
		{
			var statistics = _statisticsService.CrashesPerWeek();

			var result = statistics
				.Select(x => new {date = x.Date, value = x.Value})
				.OrderBy(x => x.date)
				.ToArray();
			return Json(result);
		}

		public ActionResult SignupsPerWeek()
		{
			var statistics = _statisticsService.SignupsPerWeek();

			var result = statistics
				.Select(x => new { date = x.Date, value = x.Value })
				.OrderBy(x => x.date)
				.ToArray();
			return Json(result);
		}

		public ActionResult ErrorsPerSignupPerWeek()
		{
			var statistics = _statisticsService.ErrorsPerUserPerWeek();

			var result = statistics
				.Select(x => new { date = x.Date, value = x.Value })
				.OrderBy(x => x.date)
				.ToArray();
			return Json(result);
		}
	}
}
