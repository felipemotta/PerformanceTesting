#addin nuget:?package=Cake.Incubator&version=3.0.0

#tool nuget:?package=Microsoft.TestPlatform&version=15.8.0
#tool nuget:?package=TestRunner&version=1.7.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

DirectoryPath projectRootPath;
SolutionParserResult solutionParserResult;
IEnumerable<CustomProjectParserResult> testProjects;

Setup(context =>
{
    projectRootPath = MakeAbsolute(Directory(".."));
    IEnumerable<string> solutionsFoundPaths = System.IO.Directory.GetFiles(projectRootPath.FullPath, "*.sln");
    if (solutionsFoundPaths.Count() != 1)
    {
        throw new CakeException("None Solution or multiple solutions were found.");
    }

    solutionParserResult = ParseSolution(solutionsFoundPaths.Single());

    testProjects = solutionParserResult
        .GetProjects()
        .Select(p => ParseProject(p.Path, configuration: configuration))
        .Where(cp => cp.IsTestProject());
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(System.IO.Path.Combine(projectRootPath.FullPath, "packages"));
    CleanDirectory(System.IO.Path.Combine(projectRootPath.FullPath, "testresults"));
    foreach (SolutionProject solutionProject in solutionParserResult.GetProjects())
    {
        CleanDirectory(System.IO.Path.Combine(solutionProject.Path.GetDirectory().FullPath, "bin"));
        CleanDirectory(System.IO.Path.Combine(solutionProject.Path.GetDirectory().FullPath, "obj"));
    }
});

Task("NuGet")
    .Does(() =>
{
    NuGetRestore(projectRootPath.FullPath);
});

Task("DotNetBuild")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings()
    {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Minimal,
        ArgumentCustomization = args => args.Append("--no-restore"),
    };

    DotNetCoreBuild(projectRootPath.FullPath, settings);
});

Task("DotNetTests")
    .Does(() =>
{
    foreach (CustomProjectParserResult testProject in testProjects)
    {
        foreach(var targetFramework in testProject.TargetFrameworkVersions)
        {
            DotNetCoreTest(testProject.ProjectFilePath.FullPath, new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                Verbosity = DotNetCoreVerbosity.Minimal,
                NoBuild = true,
                Framework = targetFramework,
                OutputDirectory = System.IO.Path.Combine(testProject.ProjectFilePath.GetDirectory().FullPath, "bin", configuration, targetFramework),
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
        }
    }
});

Task("MonoTests")
    .Does(() =>
{    
    ProcessArgumentBuilder arguments = new ProcessArgumentBuilder();
    arguments.Append(Context.Tools.Resolve("testrunner.exe").FullPath);
    foreach (CustomProjectParserResult testProject in testProjects)
    {        
        var projectDirectory = testProject.ProjectFilePath.GetDirectory();
        var runtimeFrameworks = testProject.TargetFrameworkVersions.Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"net\d{2}\d?$"));
        var testAssembles = runtimeFrameworks
            .Select(framework => System.IO.Path.Combine(projectDirectory.FullPath, "bin", configuration, framework, testProject.AssemblyName + ".dll"));
        foreach(string testFilePath in testAssembles)
        {
            arguments.Append(testFilePath);
        }
    }

    StartProcess(Context.Tools.Resolve("mono.exe"), new ProcessSettings { Arguments = arguments });
});

Task("MSBuild")
    .Does(() =>
{
    var msBuildSettings = new MSBuildSettings 
    {
        Verbosity = Verbosity.Minimal,
        ToolVersion =  MSBuildToolVersion.VS2017,
        Configuration = configuration,
        MSBuildPlatform = MSBuildPlatform.Automatic,
        WorkingDirectory = projectRootPath.FullPath
    };

    msBuildSettings.WithTarget("Build");

    MSBuild(projectRootPath.FullPath, msBuildSettings);
});

Task("VsTests")
    .Does(() =>
{
    var settings = new VSTestSettings { ToolPath = Context.Tools.Resolve("vstest.console.exe") };
    foreach (CustomProjectParserResult testProject in testProjects)
    {
        foreach(string outputPath in testProject.OutputPaths.Select(dp => System.IO.Path.Combine(dp.FullPath, "*.tests.dll")))
        {
            VSTest(outputPath, settings);
        }
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet")
    .IsDependentOn("DotNetBuild")
    .IsDependentOn("DotNetTests")
    .IsDependentOn("MonoTests");

Task("Linux")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet")
    .IsDependentOn("DotNetBuild")
    .IsDependentOn("DotNetTests")
    .IsDependentOn("MonoTests");

Task("Microsoft")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet")
    .IsDependentOn("MsBuild")
    .IsDependentOn("VsTests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
