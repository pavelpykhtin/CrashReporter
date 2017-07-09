(function() {
	angular.module('CrashReportViewer')
		.controller('DashboardController', DashboardController);

	DashboardController.$inject = ['$scope', '$http', '$location', 'httpDataProvider'];
	function DashboardController($scope, $http, $location, httpDataProvider) {
		var self = this;

		self.crashesPerWeekSubscription = null;

		self.errorsChartData = [
				{ date: 1, value: 0 },
				{ date: 2, value: 0 }
		];
		self.signupsChartData = [
				{ date: 1, value: 0 },
				{ date: 2, value: 0 }
		];
		self.errorsPerSignupChartData = [
				{ date: 1, value: 0 },
				{ date: 2, value: 0 }
		];

		self.latestProblems = [];
		self.latestCrashes = [];

		self.navigateToProblem = navigateToProblem;
		self.navigateToCrash = navigateToCrash;

		activate();

		function activate() {
			$http.post('./problemStatistics/lastWeek')
				.success(function (data) {
					self.latestProblems = data;
				});

			$http.post('./problemStatistics/latestCrashes')
				.success(function (data) {
					self.latestCrashes = data;
				});

			$scope.$on('$destroy', onDestroy);

			self.crashesPerWeekSubscription = httpDataProvider.subscribe('./problemStatistics/crashesPerWeek', onCashesReceived);
		}


		function onDestroy() {
			if (self.crashesPerWeekSubscription)
				httpDataProvider.unsubscribe(self.crashesPerWeekSubscription);
		}

		function onCashesReceived(data) {
			self.errorsChartData = data;
		}
		
		function navigateToProblem(id) {
			$location.path('/problems/' + id);
		};


		function navigateToCrash(id) {
			$location.path('/crashes/' + id);
		};
	}
})();