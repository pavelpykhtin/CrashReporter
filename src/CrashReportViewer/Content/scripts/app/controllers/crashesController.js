(function() {
	angular.module('CrashReportViewer')
		.controller('CrashesController', CrashesController);

	CrashesController.$inject = ['$routeParams', '$http', '$sce', '$location', '$modal', '$scope'];
	function CrashesController($routeParams, $http, $sce, $location, $modal, $scope) {
		var self = this;

		var pageSize = 50;

		self.pagesLoaded = 0;
		self.hasMoreData = false;

		self.refresh = refresh;
		self.loadNextPage = loadNextPage;
		self.navigateToCrash = navigateToCrash;

		activate();

		function activate() {
			self.crashes = [];

			loadNextPage();
		}

		function loadNextPage() {
			var from = self.pagesLoaded * pageSize + 1;
			var to = from + pageSize - 1;
			$http.get('./crashes/list/' + from + '-' + to)
				.then(function (response) {
					var crashes = response.data.crashes;
					self.crashes = self.crashes.concat(crashes);
					self.hasMoreData = pageSize == crashes.length;
					self.pagesLoaded++;
				});
		}

		function refresh() {
			var from = 1;
			var to = self.pagesLoaded * pageSize;

			$http.get('./crashes/list/' + from + '-' + to)
				.then(function (response) {
					var crashes = response.data.crashes;
					self.crashes = crashes;
					self.hasMoreData = (self.pagesLoaded + 1) * pageSize == crashes.length;
				});
		}

		function navigateToCrash(crashId) {
			$location.path('/crashes/' + crashId);
		}
	}
})();
