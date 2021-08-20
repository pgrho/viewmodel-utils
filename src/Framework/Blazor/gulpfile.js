/// <binding BeforeBuild='default' Clean='clean' />
var gulp = require("gulp");
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var ts = require('gulp-typescript');

gulp.task('clean', function () {
    return del(['wwwroot/*.js']);
});
gulp.task('tsc', function () {
    return gulp.src(['Scripts/*.ts']).pipe(ts({
        outFile: 'all.js'
    })).pipe(gulp.dest('Scripts/'));
});
gulp.task('scripts', function () {
    return gulp.src([
        'Scripts/Copyright.js',
        'Scripts/all.js'
    ])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.js'))
        .pipe(uglify({
            output: {
                comments: /^!/
            }
        }))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('default', gulp.series(['clean', 'tsc', 'scripts']));