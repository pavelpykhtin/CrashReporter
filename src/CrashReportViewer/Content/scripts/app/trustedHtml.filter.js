(function () {
	angular.module('CrashReportViewer')
		.filter('trusted', TrustedHtmlFilter);

	TrustedHtmlFilter.$inject = ['$sce'];
	function TrustedHtmlFilter($sce) {
		var trustedHtml = function (rawHtml) {
			return $sce.trustAsHtml(rawHtml);
		};

		trustedHtml.$stateful = true;

		return trustedHtml;
	}
})();