/// <binding />
'use strict';

var gulp = require('gulp');

gulp.task('jq', function () {
    return gulp.src("./node_modules/jquery/dist/*").pipe(gulp.dest('wwwroot/jquery'));
});

gulp.task('default', gulp.parallel('jq'));