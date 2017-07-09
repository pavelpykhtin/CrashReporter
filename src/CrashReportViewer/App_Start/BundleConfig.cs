using System.Web.Optimization;

namespace CrashReportViewer.App_Start
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/angularjs")
				.Include("~/Content/Scripts/libs/angular/angular.js")
				.Include("~/Content/Scripts/libs/angular/angular-animate.js")
				.Include("~/Content/Scripts/libs/angular/angular-route.js"));

			bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap")
				.Include("~/Content/Scripts/libs/ui-bootstrap-tpls-0.13.0.js"));
		}
	}
}