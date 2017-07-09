(function() {
	'use strict';

	angular.module('charts')
		.service('httpDataProvider', HttpDataProvider);

	HttpDataProvider.$inject = ['$http', '$interval'];
	function HttpDataProvider($http, $interval) {
		var self = this;

		self.subscribe = subscribe;
		self.unsubscribe = unsubscribe;
		
		function subscribe(url, callback) {
			return new HttpDataSubscription(url, callback, true, $http, $interval);
		}

		function unsubscribe(subscription) {
			subscription.destroy();
		}
	}

	function HttpDataSubscription(url, callback, runOnCreation, $http, $interval) {
		var self = this;

		var timer = null;

		self.destroy = destroy;

		activate();

		function activate() {
			if(runOnCreation)
				start();
		}

		function start() {
			timer = $interval(onTick, 60000);
			onTick();
		}

		function destroy() {
			if (!timer)
				return;

			$interval.cancel(timer);

			timer = null;
		}

		function onTick() {
			$http.post(url)
				.then(function(response) {
					if (callback)
						callback(response.data);
				});
		}
	}
})();
