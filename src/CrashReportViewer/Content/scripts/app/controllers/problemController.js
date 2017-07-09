(function() {
	angular.module('CrashReportViewer')
		.controller('ProblemController', ProblemController);

	ProblemController.$inject = ['$routeParams', '$http', '$location', '$modal', '$scope'];
	function ProblemController($routeParams, $http, $location, $modal, $scope) {
		var self = this;

		self.id = $routeParams.id;
		self.problemDescriptionCollapsed = true;
		self.problem = {
			shortDescription: '',
			description: '',
			collapsedDescription: ''
		};
		self.crashes = [];
		self.relatedProblems = [];

		self.refresh = function() {
			$http.post('./problems', { id: self.id })
				.success(function(data) {
					self.problem = data;

					self.problem.collapsedDescription = data.description.substr(0, 200);

					self.problem.description = data.description;
					self.problem.collapsedDescription = data.collapsedDescription;
				});
			$http.post('./crashes/byProblem', { problemId: self.id })
				.success(function(data) {
					self.crashes = data;
				});
			$http.post('./problems/related', { problemId: self.id })
				.then(function(response) {
					self.relatedProblems = response.data;
				});
		};

		self.toggleDescription = function() {
			self.problemDescriptionCollapsed = !self.problemDescriptionCollapsed;
		};

		self.navigateToCrash = function(crashId) {
			$location.path('/crashes/' + crashId);
		};

		self.navigateToProblem = function (problemId) {
			$location.path('/problems/' + problemId);
		};

		self.edit = function () {
			var dialogScope = $scope.$new();
			dialogScope.id = self.id;

			$modal
				.open({
					templateUrl: './content/templates/problemEditing.htm',
					scope: dialogScope
				})
				.result
				.then(function() {
					self.refresh();
				});
		};

		self.postToRedmine = function() {
			$http.post('./problems/postToRedmine', { id: self.id })
				.then(function () {
					self.refresh();
				});
		};

		self.refresh();
	}
})();