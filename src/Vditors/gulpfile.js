/// <binding />
'use strict';

var gulp = require('gulp');

gulp.task('vditor', function () {
    return gulp.src("./node_modules/vditor/dist/index.{css,min.js}").pipe(gulp.dest('wwwroot/vditor'));
});

gulp.task('default', gulp.parallel('vditor'));