var gulp = require("gulp");
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var ts = require('gulp-typescript');

var jtToast = process.env['PkgBlazorJqueryToast'];
var typeahead = process.env['PkgBlazorTypeahead'];

gulp.task('clean', function () {
    return del(['wwwroot/*.js', 'Scripts/Shipwreck.ViewModelUtils.Blazor.js']);
});
gulp.task('tsc', function () {
    return gulp.src(['Scripts/*.ts']).pipe(ts({
        outFile: 'Shipwreck.ViewModelUtils.Blazor.js'
    })).pipe(gulp.dest('Scripts/'));
});
gulp.task('concatjs', function () {
    return gulp.src(['Scripts/Copyright.js', 'Scripts/Shipwreck.ViewModelUtils.Blazor.js'])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('minifyjs', function () {
    return gulp.src([
        'wwwroot/Shipwreck.ViewModelUtils.Blazor.js'
    ])
        .pipe(uglify({
            output: {
                comments: /^!/
            }
        }))
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.min.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('bundlejs', function () {
    return gulp.src([
        'node_modules/jquery/dist/jquery.min.js',
        'node_modules/popper.js/dist/umd/popper-utils.min.js',
        'node_modules/popper.js/dist/umd/popper.min.js',
        'node_modules/bootstrap/dist/js/bootstrap.min.js',
        'node_modules/moment/min/moment-with-locales.min.js',
        'node_modules/tempusdominus-bootstrap-4/build/js/tempusdominus-bootstrap-4.min.js',
        jtToast + '\\staticwebassets\\Shipwreck.BlazorJqueryToast.js',
        typeahead + '\\staticwebassets\\Shipwreck.BlazorTypeahead.js',
        '../../Core/Blazor/wwwroot/Shipwreck.ViewModelUtils.Core.Blazor.min.js',
        'wwwroot/Shipwreck.ViewModelUtils.Blazor.min.js'
    ])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.bundle.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('bundlecss', function () {
    return gulp.src([
        'node_modules/bootstrap/dist/css/bootstrap.min.css',
        'node_modules/tempusdominus-bootstrap-4/build/css/tempusdominus-bootstrap-4.min.css',
        jtToast + '\\staticwebassets\\Shipwreck.BlazorJqueryToast.css' 
    ])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.bundle.css'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('default', gulp.series(['clean', 'tsc', 'concatjs', 'minifyjs', 'bundlejs']));