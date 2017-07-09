using System.Web.Mvc;
using CrashReport.Infrastructure.Nhibernate;

namespace CrashReportViewer.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISessionProvider _sp;

		public HomeController(ISessionProvider sp)
		{
			_sp = sp;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}
