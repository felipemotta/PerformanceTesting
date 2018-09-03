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

Task("Build")
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

Task("Mono")
    .Does(() =>
{    
    ProcessArgumentBuilder arguments = new ProcessArgumentBuilder();
    arguments.Append(Context.Tools.Resolve("testrunner.exe").FullPath);
    
    foreach (CustomProjectParserResult testProject in testProjects)
    {
        foreach(string outputPath in testProject.OutputPaths.Select(dp => System.IO.Path.Combine(dp.FullPath, testProject.AssemblyName + ".dll")))
        {
            Information("Running '{0}' output ...", outputPath);
            arguments.Append(outputPath);
        }
    }

    StartProcess(Context.Tools.Resolve("mono.exe"), new ProcessSettings { Arguments = arguments });

});

Task("Tests")
    .Does(() =>
{
    var settings = new VSTestSettings { ToolPath = Context.Tools.Resolve("vstest.console.exe") };
    foreach (CustomProjectParserResult testProject in testProjects)
    {
        foreach(string outputPath in testProject.OutputPaths.Select(dp => System.IO.Path.Combine(dp.FullPath, "*.tests.dll")))
        {
            Information("Running '{0}' output ...", outputPath);
            VSTest(outputPath, settings);
        }
    }
});

Task("DotNetTests")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Minimal,
        ArgumentCustomization = args => args.Append("--no-restore"),
    };    

    foreach (CustomProjectParserResult testProject in testProjects)
    {
        Information("Running '{0}' project ...", testProject.AssemblyName);
        DotNetCoreTest(testProject.ProjectFilePath.FullPath, settings);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet")
    .IsDependentOn("Build")
    .IsDependentOn("Tests")
    .IsDependentOn("DotNetTests")
    .IsDependentOn("Mono");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);