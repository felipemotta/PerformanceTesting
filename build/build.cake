#addin nuget:?package=Cake.Incubator&version=3.0.0

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

Setup(context =>
{
    projectRootPath = MakeAbsolute(Directory(".."));
    IEnumerable<string> solutionsFoundPaths = System.IO.Directory.GetFiles(projectRootPath.FullPath, "*.sln");
    if (solutionsFoundPaths.Count() != 1)
    {
        throw new CakeException("None Solution or multiple solutions were found.");
    }

    solutionParserResult = ParseSolution(solutionsFoundPaths.Single());
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(System.IO.Path.Combine(projectRootPath.FullPath, "packages"));
    CleanDirectory(System.IO.Path.Combine(projectRootPath.FullPath, "testresults"));

    // Clean projects outputs
    IEnumerable<SolutionProject> solutionProjects = solutionParserResult.GetProjects();
    foreach (SolutionProject solutionProject in solutionProjects)
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
    var msBuildSettings = new MSBuildSettings {
			Verbosity = Verbosity.Minimal,
			ToolVersion =  MSBuildToolVersion.VS2017,
			Configuration = configuration,
			PlatformTarget = PlatformTarget.MSIL,
			MSBuildPlatform = MSBuildPlatform.Automatic,
			WorkingDirectory = projectRootPath.FullPath
		};

		msBuildSettings.WithTarget("Build");

		MSBuild(projectRootPath.FullPath, msBuildSettings);
});

Task("Tests")
    .Does(() =>
{
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet")
    .IsDependentOn("Build")
    .IsDependentOn("Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);