var gulp = require("gulp");
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var ts = require('gulp-typescript');
var sass = require('gulp-sass')(require('sass'));
var cleanCss = require('gulp-clean-css');
var tsProject = ts.createProject('Scripts/tsconfig.json');

function getArgs(name) {
    const i = process.argv.indexOf(name);
    return i >= 0 ? process.argv[i + 1] : null;
}

var jtToast = getArgs('--toast');
var typeahead = getArgs('--typeahead');

gulp.task('clean', function () {
    return del(['wwwroot/*.js', 'wwwroot/*.css', 'Scripts/Shipwreck.ViewModelUtils.Blazor.js']);
});
gulp.task('tsc', function () {
    return gulp.src([
        'node_modules/@types/jquery/**/*.d.ts',
        'node_modules/popper.js/index.d.ts',
        'Scripts/*.ts'
    ]).pipe(ts({
        rootDir: "Scripts/",
        outFile: 'Shipwreck.ViewModelUtils.Blazor.js'
    }, ts.reporter.longReporter())).pipe(gulp.dest('Scripts/'));
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
        jtToast + 'Shipwreck.BlazorJqueryToast.js',
        typeahead + 'Shipwreck.BlazorTypeahead.js',
        '../../Core/Blazor/wwwroot/Shipwreck.ViewModelUtils.Core.Blazor.min.js',
        'wwwroot/Shipwreck.ViewModelUtils.Blazor.min.js'
    ])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.bundle.js'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('fwcss', function () {
    return gulp.src(['Styles/Copyright.scss', 'Styles/*.scss'])
        .pipe(sass())
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.min.css'))
        .pipe(cleanCss())
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('bundlecss', function () {
    return gulp.src([
        'node_modules/bootstrap/dist/css/bootstrap.min.css',
        'node_modules/tempusdominus-bootstrap-4/build/css/tempusdominus-bootstrap-4.min.css',
        jtToast + 'Shipwreck.BlazorJqueryToast.css',
        'wwwroot/Shipwreck.ViewModelUtils.Blazor.min.css'
    ])
        .pipe(concat('Shipwreck.ViewModelUtils.Blazor.bundle.css'))
        .pipe(gulp.dest('wwwroot/'));
});
gulp.task('default', gulp.series(['clean', 'tsc', 'concatjs', 'minifyjs', 'bundlejs', 'fwcss', 'bundlecss']));