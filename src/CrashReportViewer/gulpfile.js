/// <binding BeforeBuild='build' ProjectOpened='default' />
(function() {
	'use strict';
	var gulp = require('gulp');
	var gutil = require('gulp-util');
	var uglify = require('gulp-uglify');
	var sourcemaps = require('gulp-sourcemaps');
	var concat = require('gulp-concat');
	var clean = require('gulp-clean');
	var cssMinify = require('gulp-minify-css');
	var less = require('gulp-less');
	var rename = require('gulp-rename');
	var gulpIf = require('gulp-if');

	var scriptsSrc = './Content/scripts/app/**/*.js';
	var scriptsDest = './Content/scripts/prod';
	var stylesWatch = './Content/styles/**/*.less';
	var stylesSrc = './Content/styles/main.less';
	var stylesDest = './Content/styles/prod';

	var useUglifier = false;

	gulp.task('default', function() {
		gulp.watch(scriptsSrc, ['scripts']);
		gulp.watch(stylesWatch, ['styles']);
	});

	gulp.task('build', ['clean', 'scripts', 'styles']);

	gulp.task('scripts', function() {
		gulp
			.src(scriptsSrc)
			.pipe(sourcemaps.init())
			.pipe(gulpIf(useUglifier, uglify()))
			.pipe(concat('app.js'))
			.pipe(sourcemaps.write('./maps'))
			.pipe(gulp.dest(scriptsDest));
	});

	gulp.task('styles', function() {
		gulp
			.src(stylesSrc)
			.pipe(less())
			.pipe(cssMinify())
			.pipe(rename('app.min.css'))
			.pipe(gulp.dest(stylesDest));
	});

	gulp.task('clean', function() {
		gulp
			.src(scriptsDest, { read: false })
			.pipe(clean());
	});
})();
