/// <binding />
'use strict';

var gulp = require('gulp');

gulp.task('mdi', function () {
    return gulp.src("./node_modules/@mdi/font/{css,fonts}/**/*").pipe(gulp.dest('wwwroot/mdi'));
});

gulp.task('default', gulp.parallel('mdi'));