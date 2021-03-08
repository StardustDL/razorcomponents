/// <binding />
'use strict';

var gulp = require('gulp');

gulp.task('font', function () {
    return gulp.src("./node_modules/@mdi/font/{css,fonts}/**/*").pipe(gulp.dest('wwwroot/mdi/font'));
});

gulp.task('svg', function () {
    return gulp.src("./node_modules/@mdi/svg/svg/**/*").pipe(gulp.dest('wwwroot/mdi/svg'));
});

gulp.task('default', gulp.parallel('font', 'svg'));