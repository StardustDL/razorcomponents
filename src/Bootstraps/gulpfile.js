/// <binding />
'use strict';

var gulp = require('gulp');

gulp.task('bootstrap', function () {
    return gulp.src("./node_modules/bootstrap/dist/**/*").pipe(gulp.dest('wwwroot/bootstrap'));
});

gulp.task('default', gulp.parallel('bootstrap'));