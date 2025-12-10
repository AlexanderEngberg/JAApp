#addin nuget:?package=Cake.Yaml&version=6.0.0

var target = Argument("target", "Basic");
var configuration = Argument("configuration", "Release");
var webServerPath = "./src/Web";

Task("Build")
    .Does(() =>
    {
       Information("Building Project...");
       DotNetBuild("./JAApp.slnx", new DotNetBuildSettings
       {
          Configuration = configuration 
       });
    });

Task("Test")
    .Does(() =>
    {
       Information("Testing project...");
       DotNetTest("./JAApp.slnx", new DotNetTestSettings
       {
          Configuration = configuration,
          NoBuild = true 
       });
    });

Task("Basic")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

RunTarget(target);