(function() {
	angular.module('CrashReportViewer')
		.controller('ProblemsController', ProblemsController);

	ProblemsController.$inject = ['$routeParams', '$http', '$sce', '$location', '$modal', '$scope'];
	function ProblemsController($routeParams, $http, $sce, $location, $modal, $scope) {
		var self = this;

		var pageSize = 50;

		self.pagesLoaded = 0;
		self.hasMoreData = false;

		self.refresh = refresh;
		self.loadNextPage = loadNextPage;
		self.navigateToProblem = navigateToProblem;
		self.doDeploy = doDeploy;

		activate();

		function activate() {
			self.problems = [];

			loadNextPage();
		}

		function loadNextPage() {
			var from = self.pagesLoaded * pageSize + 1;
			var to = from + pageSize - 1;
			$http.get('./problems/list/' + from + '-' + to)
				.then(function (response) {
					var problems = response.data.problems;
					self.problems = self.problems.concat(problems);
					self.hasMoreData = pageSize == problems.length;
					self.pagesLoaded++;
				});
		}

		function refresh() {
			var from = 1;
			var to = self.pagesLoaded * pageSize;

			$http.get('./problems/list/' + from + '-' + to)
				.then(function (response) {
					var problems = response.data.problems;
					self.problems = problems;
					self.hasMoreData = (self.pagesLoaded + 1) * pageSize == problems.length;
				});
		}

		function doDeploy() {
			$http.post('./problems/markAsDeployed')
				.then(function () {
					refresh();
				});
		}

		function navigateToProblem(problemId) {
			$location.path('/problems/' + problemId);
		}
	}
})();
