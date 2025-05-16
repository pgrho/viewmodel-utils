var gulp = require("gulp");
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var ts = require('gulp-typescript');

gulp.task('clean', function () {
    return del(['wwwroot/*.js', 'Scripts/Shipwreck.ViewModelUtils.Core.Blazor.js']);
});
gulp.task('tsc', function () {
    return gulp.src(['Scripts/*.ts']).pipe(ts({
        outFile: 'Shipwreck.ViewModelUtils.Core.Blazor.js'
    })).pipe(gulp.dest('Scripts/'));
});
gulp.task('concatjs', function () {
    return gulp.src(['Scripts/Copyright.js', 'Scripts/Shipwreck.ViewModelUtils.Core.Blazor.js'])
        .pipe(concat('Shipwreck.ViewModelUtils.Core.Blazor.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('minifyjs', function () {
    return gulp.src([
        'wwwroot/Shipwreck.ViewModelUtils.Core.Blazor.js'
    ])
        .pipe(uglify({
            output: {
                comments: /^!/
            }
        }))
        .pipe(concat('Shipwreck.ViewModelUtils.Core.Blazor.min.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('default', gulp.series(['clean', 'tsc', 'concatjs', 'minifyjs']));