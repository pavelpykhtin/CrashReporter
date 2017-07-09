(function() {
	angular.module('CrashReportViewer')
		.controller('ProblemEditController', ProblemEditController);
	
	ProblemEditController.$inject = ['$scope', '$http'];
	function ProblemEditController($scope, $http) {
		var self = this;

		self.problem = {
			status: '0',
			shortDescription: '',
			description: ''
		};

		self.cancel = cancel;
		self.accept = accept;

		activate();

		function activate() {
			self.id = $scope.id;

			self.statuses = [
				{ id: 0, text: 'New' },
				{ id: 1, text: 'Fixed' },
				{ id: 2, text: 'Deployed' },
				{ id: 3, text: 'Repeated' }
			];

			$http.get('./problems/edit/' + self.id)
				.success(function(data) {
					self.problem = data;
				});
		}

		function cancel() {
			$scope.$dismiss();
		}

		function accept() {
			$http.post('./problems/edit/' + self.id, self.problem)
				.success(function() {
					return $scope.$close();
				});
		}
	}
})();