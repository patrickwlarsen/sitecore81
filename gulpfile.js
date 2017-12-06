var gulp = require("gulp");
var cssmin = require("gulp-cssmin");
var rename = require("gulp-rename");
var concat = require("gulp-concat");
var uglify = require("gulp-uglify");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var gulpConfig = require("./gulp-config.js")();
module.exports.config = gulpConfig;
gulp.task("Publish-Site", function () {
    return gulp.src("./{Feature,Foundation,Project}/**/**/*.csproj")
        .pipe(foreach(function (stream, file) {
            return stream
                .pipe(debug({ title: "Publishing " }))
                .pipe(msbuild({
                    targets: ["Build"],
                    gulpConfiguration: gulpConfig.buildConfiguration,
                    properties: {
                        publishUrl: gulpConfig.webRoot,
                        DeployDefaultTarget: "WebPublish",
                        WebPublishMethod: "FileSystem",
                        DeployOnBuild: "true",
                        DeleteExistingFiles: "false",
                        _FindDependencies: "false",
                        VisualStudioVersion: "15.0"
                    },
                    verbosity: "diagnostic",
                    toolsVersion: 15.0
                }));
        }));
});

var minifyCss = function (destination) {
    gulp.src("./{Feature,Foundation,Project}/**/**/Content/*.css")
        .pipe(concat('sitecoredev.website.min.css'))
        .pipe(cssmin())
        .pipe(gulp.dest(destination));
};

var minifyJs = function (destination) {
    gulp.src("./{Feature,Foundation,Project}/**/**/Scripts/*.js")
        .pipe(concat('sitecoredev.website.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(destination));
};

gulp.task("minify-css", function () {
    minifyCss();
});

gulp.task("minify-js", function () {
    minifyJs();
});

gulp.task("css-watcher", function () {
    var root = "./";
    var roots = [root + "**/Content", "!" + root + "/**/obj/**/Content"];
    var files = "/**/*.css";
    var destination = gulpConfig.webRoot + "\\Content";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    minifyCss(destination);
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});

gulp.task("js-watcher", function () {
    var root = "./";
    var roots = [root + "**/Scripts", "!" + root + "/**/obj/**/Scripts"];
    var files = "/**/*.js";
    var destination = gulpConfig.webRoot + "\\Scripts";
    gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, rootFolder) {
            gulp.watch(rootFolder.path + files, function (event) {
                if (event.type === "changed") {
                    console.log("publish this file " + event.path);
                    minifyJs(destination);
                }
                console.log("published " + event.path);
            });
            return stream;
        })
    );
});