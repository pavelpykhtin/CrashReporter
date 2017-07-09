(function() {
	'use strict';

	angular.module('charts')
		.directive('d3LineChart', D3LineChart);

	D3LineChart.$inject = [];
	function D3LineChart() {
		return {
			restrict: 'E',
			template: '<svg></svg>',
			controller: D3LineChartController
		};
	}

	D3LineChartController.$inject = ['$scope', '$element', '$attrs'];
	function D3LineChartController($scope, $element, $attrs) {
		var el = null;
		var chart = null;

		var width = 10;
		var height = 10;
		var margin = {
			left: 10,
			top: 10,
			right: 40,
			bottom: 20
		};

		activate();

		function activate() {
			el = $element;

			width = $(el).width() - margin.left - margin.right;
			height = $(el).height() - margin.top - margin.bottom;

			chart = initChart();
			update($scope.$eval($attrs.chartData));

			$scope.$watch($attrs.chartData, onChartDataUpdated);
		}

		function onChartDataUpdated(newValue, oldValue) {
			update(newValue);
		}

		function initChart() {
			var x = d3.scale.linear()
				.range([0, width]);
			var y = d3.scale.linear()
				.range([height, 0]);
			return {
				svg: d3.select(el[0]).select('svg')
					.attr('width', width + margin.left + margin.right)
					.attr('height', height + margin.top + margin.bottom)
					.append('g')
					.classed('chart-area', true)
					.attr('transform', 'translate(' + margin.left + ', ' + margin.top + ')'),
				x: x,
				y: y,
				xAxis: d3.svg.axis()
					.scale(x)
					.tickSize(-height)
					.tickFormat(formatDate)
					.tickPadding(6),
				yAxis: d3.svg.axis()
					.scale(y)
					.ticks(4)
					.orient("right"),
				line: d3.svg.line()
					.interpolate("monotone")
					.x(function (d) { return x(d.date); })
					.y(function (d) { return y(d.value); })
			}

		}

		function update(data) {
			chart.svg.datum(data);

			chart.x.domain([data[0].date, data[data.length - 1].date]);
			chart.y.domain([0, d3.max(data, function (d) { return d.value; })]).nice();

			chart.svg.selectAll('.axis,.line').remove();

			chart.svg.append("g")
				.attr("class", "x axis")
				.attr("transform", "translate(0," + height + ")")
				.call(chart.xAxis);

			chart.svg.append("g")
				.attr("class", "y axis")
				.attr("transform", "translate(" + width + ",0)")
				.call(chart.yAxis);

			chart.svg.append("path")
				.attr("class", "line")
				.attr("d", chart.line);
		}

		function formatDate(tick) {
			var d = new Date();
			d.setDate(d.getDate() + tick);

			return d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
		}
	}
})();
