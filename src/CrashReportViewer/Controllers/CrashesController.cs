using System;
using System.Web.Mvc;
using System.Linq;
using CrashReport.Infrastructure.Repositories;
using CrashReportViewer.Utils;

namespace CrashReportViewer.Controllers
{
	public class CrashesController: Controller
	{
		private readonly ICrashRepository _crashRepository;

		public CrashesController(ICrashRepository crashRepository)
		{
			_crashRepository = crashRepository;
		}

		public ActionResult ByProblem(int problemId)
		{
			var errors = _crashRepository.GetByProblemId(problemId, 10);

			return Json(errors
				.Select(x => new
						    {
						     	id = x.Id,
						     	date = x.Timestamp.ToString("dd.MM.yyyy HH:mm:ss"),
						     	version = x.Version,
                                message = x.Message
						    })
				.ToArray());
		}

		public ActionResult Index(int id)
		{
			var error = _crashRepository.GetById(id);

			return Json(new
							{
								problem = new
											{
												id = error.Problem.Id,
												shortDescription = error.Problem?.ShortDescription
											},
								crash = new
								        	{
								        		id = error.Id,
												module = error.Application.Name,
												version = error.Version,
												message = error.Message,
												stackTrace = error.StackTrace.FixLineBreaks(),
												additionalInformation = error.AdditionalInformation.FixLineBreaks(),
												innerException = error.InnerException.FixLineBreaks(),
												date = error.Timestamp.ToString("dd.MM.yyyy HH:mm:ss")
								        	}
							});
		}

		[HttpGet]
		public ActionResult List(int from = 1, int? to = null)
		{
			return Json(
				new
				{
					crashes = _crashRepository.GetLatestCrashes(from, to ?? 20)
						.Select(c =>
						{
							var shortDescription = c.Problem?.ShortDescription;
							shortDescription = shortDescription?.Substring(0, Math.Min(50, shortDescription.Length));

							return new
							{
								module = c.Problem?.Application?.Name,
								id = c.Id,
								version = c.Problem?.Status?.ToString(),
								shortDescription = shortDescription,
								problemId = c.Problem?.Id,
								date = c.Timestamp.ToString("dd.MM.yyyy HH:mm")
							};
						}).ToArray()
				}, JsonRequestBehavior.AllowGet);
		}


	}
}