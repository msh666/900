/// <binding Clean='build' ProjectOpened='default' />
var gulp = require('gulp'),
    sass = require('gulp-sass'),
    sourcemaps = require('gulp-sourcemaps'),
    autoprefixer = require('gulp-autoprefixer'),
    cleanCSS = require('gulp-clean-css'),
    debug = require('gulp-debug'),
    livereload = require('gulp-livereload');


var sassFile = "wwwroot/css/site.scss",
    sassWildcard = "wwwroot/css/**/*.scss";

var cssDest = "wwwroot/css/";

gulp.task('sass',
    function() {
        return gulp.src(sassFile)
            .pipe(sourcemaps.init())
            .pipe(sass().on('error', sass.logError))
            .pipe(autoprefixer())
            .pipe(cleanCSS())
            .pipe(sourcemaps.write("."))
            .pipe(gulp.dest(cssDest))
            .pipe(livereload());
    });


gulp.task("watch",
    function() {
        livereload.listen();
        var listen = function() {
            gulp.watch(sassWildcard, ['sass']);
        };
        listen();
    });

// Build
gulp.task('build', ['sass']);
gulp.task('default', ['build', 'watch']);