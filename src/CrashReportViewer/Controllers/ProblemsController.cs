using System;
using System.Linq;
using System.Web.Mvc;
using CrashReportViewer.Models;
using CrashReport.Domain;
using CrashReport.Infrastructure.Repositories;
using CrashReportViewer.Infrastructure.RedmineIntegration;
using CrashReportViewer.Utils;

namespace CrashReportViewer.Controllers
{
	public class ProblemsController: Controller
	{
		private readonly IProblemRepository _problemRepository;
		private readonly IRedmineIntegrationService _redmineService;

		public ProblemsController(
			IProblemRepository problemRepository, 
			IRedmineIntegrationService redmineService)
		{
			_problemRepository = problemRepository;
			_redmineService = redmineService;
		}

		public ActionResult Index(int id)
		{
			var problem  = _problemRepository.GetById(id);

			return Json(new
			            	{
								id = problem.Id,
								module = problem.Application?.Name,
								status = problem.Status.ToString(),
								fixedInVersion = problem.FixedInVersion,
								shortDescription = problem.ShortDescription,
			            		description = problem.Description.FixLineBreaks(), 
								externalId = problem.ExternalId
			            	});
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{
			var problem = _problemRepository.GetById(id);

			return Json(new ProblemEditPe
			            	{
								status = problem.Status,
								fixedInVersion = problem.FixedInVersion,
								shortDescription = problem.ShortDescription,
								description = problem.Description
			            	}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Edit(int id, ProblemEditPe pe)
		{
			var problem = _problemRepository.GetById(id);

			problem.Status = pe.status;
			problem.Description = pe.description;
			problem.FixedInVersion = pe.fixedInVersion;
			problem.ShortDescription = pe.shortDescription;

			_problemRepository.Save(problem);

			return Json("ok");
		}

		[HttpGet]
		public ActionResult List(int from = 1, int? to = null)
		{
			return Json(
				new
					{
						problems = _problemRepository.GetLatestProblems(from, to ?? 20)
						.Select(c =>
						{
							var shortDescription = c.Problem?.ShortDescription;
							shortDescription = shortDescription?.Substring(0, Math.Min(50, shortDescription.Length));

							return new
							{
								module = c.Problem?.Application?.Name,
								id = c.Problem?.Id,
								status = c.Problem?.Status?.ToString(),
								shortDescription = shortDescription,
								lastOccurence = c.Timestamp.ToString("dd.MM.yyyy HH:mm")
							};
						}).ToArray()
					}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult Related(int problemId)
		{
			var relatedProblems = _problemRepository.GetRelatedProblems(problemId);

			var data = relatedProblems
				.Select(p => 
				{
					var shortDescription = p.ShortDescription;
					shortDescription = shortDescription?.Substring(0, Math.Min(50, shortDescription.Length));

					return new
					{
						module = p.Application?.Name,
						id = p.Id,
						status = p.Status?.ToString(),
						shortDescription = shortDescription
					};
				});

			return Json(data);
		}

		[HttpPost]
		public ActionResult MarkAsDeployed()
		{
			var problemsToMark = _problemRepository.GetFixedProblems();

			foreach (var problem in problemsToMark)
				problem.Status = EProblemStatus.Deployed;

			return Content("ok");
		}

		[HttpPost]
		public ActionResult PostToRedmine(int id)
		{
			var problem = _problemRepository.GetById(id);

			var problemUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/#/problems/{id}";
			var issueUrl = _redmineService.CreateIssue(problem, "ow-web", problemUrl);

			problem.ExternalId = issueUrl;

			return Index(id);
		}
	}
}