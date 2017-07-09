(function () {
	angular.module('CrashReportViewer')
		.controller('CrashController', CrashController);

	CrashController.$inject = ['$routeParams', '$http'];
	function CrashController($routeParams, $http) {
		var self = this;

		self.id = $routeParams.id;
		self.stackTraceCollapsed = true;
		self.innerExceptionCollapsed = true;
		self.problem = {
			shortDescription: '',
			id: null
		};
		self.crash = {
			module: null,
			date: null,
			message: null,
			additionalInformation: null,
			userId: null,
			peopleId: null,
			stackTrace: null,
			collapsedStackTrace: null,
			collapsedInnerException: null
		};

		$http.post('./crashes', { id: self.id })
			.success(function (data) {
				self.problem = data.problem;
				self.crash = data.crash;

				self.crash.collapsedStackTrace = (data.crash.stackTrace || '').substr(0, 200);
				self.crash.collapsedInnerException = (data.crash.innerException || '').substr(0, 200);
				self.additionalInformation = JSON.parse(data.crash.additionalInformation);

				self.crash.stackTrace = data.crash.stackTrace;
				self.crash.collapsedStackTrace = data.crash.collapsedStackTrace;
				self.crash.innerException = data.crash.innerException;
				self.crash.collapsedInnerException = data.crash.collapsedInnerException;
			});

		self.toggleStackTrace = function () {
			self.stackTraceCollapsed = !self.stackTraceCollapsed;
		};

		self.toggleInnerException = function () {
			self.innerExceptionCollapsed = !self.innerExceptionCollapsed;
		};
	}
})();