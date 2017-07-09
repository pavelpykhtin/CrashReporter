(function () {
	var app = angular.module('CrashReportViewer', ['ngRoute', 'ui.bootstrap', 'charts', 'jsonFormatter']);
	//app.constant('ngCrashReportSettings', {
	//	url: application.logger.url,
	//	application: application.logger.applicationKey,
	//	applicationVersion: application.version
	//});

	app.config(['$routeProvider', function ($routeProvider) {
		$routeProvider
			.when('/', {
				templateUrl: './Content/templates/dashboard.htm'
			})
			.when('/home', {
				templateUrl: './Content/templates/dashboard.htm'
			})
			.when('/problems/:id*', {
				templateUrl: './Content/templates/problem.htm'
			})
			.when('/problems', {
				templateUrl: './Content/templates/problems.htm'
			})
			.when('/crashes/:id*', {
				templateUrl: './Content/templates/crash.htm'
			})
			.when('/crashes', {
				templateUrl: './Content/templates/crashes.htm'
			});
	}]);
})();