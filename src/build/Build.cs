using NuGet.Versioning;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using System;
using System.IO;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
	[Solution] readonly Solution Solution;
	[GitRepository] readonly GitRepository GitRepository;
	[GitVersion] readonly GitVersion GitVersion;

	[Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
	[Parameter] string NugetApiKey;

	public static int Main()
	{
		return Execute<Build>(x => x.Compile);
	}

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	AbsolutePath LocalNugetPath => ArtifactsDirectory / "localnuget";

	AbsolutePath SourceDirectory => RootDirectory / "src";
	AbsolutePath McProtoNetDirectory => SourceDirectory / "McProtoNet";
	AbsolutePath TestsDirectory => RootDirectory / "tests";
	AbsolutePath AnalyzerDirectory => RootDirectory / "analyzers";
	AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
	AbsolutePath NugetDirectory => ArtifactsDirectory / "nuget";

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{

		});

	Target Restore => _ => _
		.Executes(() =>
		{

		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Requires(() => Configuration.Equals(Configuration.Debug))
		.Executes(() =>
		{

		});


	Target BuildSDKToLocal => _ => _
		.Executes(() =>
		{

			NugetDirectory.DeleteDirectory();

			string NuGetVersionCustom = GitVersion.NuGetVersion;
			int commitNum = 0;
			if (Int32.TryParse(GitVersion.CommitsSinceVersionSource, out commitNum))
			{

			}

			NuGetVersionCustom = NuGetVersionCustom + "-" + "local." + commitNum;

			string outputPackagePath = NugetDirectory / ("HolyClient.SDK." + NuGetVersionCustom + ".nupkg");


			DotNetPack(s => s
			   .SetProject(Solution.GetProject("HolyClient.Abstractions"))
			   .SetConfiguration(Configuration)
			   .EnableNoBuild()
			   .EnableNoRestore()
			   .SetVersion(NuGetVersionCustom)
			   .SetNoDependencies(true)
			   .SetOutputDirectory(NugetDirectory));

			DotNetPack(s => s
			   .SetProject(Solution.GetProject("HolyClient.SDK"))
			   .SetConfiguration(Configuration)
			   .EnableNoBuild()
			   .EnableNoRestore()
			   .SetVersion(NuGetVersionCustom)
			   .SetNoDependencies(true)
			   .SetOutputDirectory(NugetDirectory));

			LocalNugetPath.CreateDirectory();

			

			NugetDirectory.GlobFiles("*.nupkg")
				  .NotEmpty()
				  // .Where(x => !x.EndsWith("symbols.nupkg"))
				  .ForEach(x =>
				  {
					  DotNetNuGetPush(s => s
						  .SetTargetPath(x)
						  .SetSource(LocalNugetPath)
					  );
				  });

		});

	Target SDKPublishToNuGet => _ => _
		.Requires(() => NugetApiUrl)
		.Requires(() => NugetApiKey)
		.Requires(() => Configuration.Equals(Configuration.Release))
		.Executes(() =>
		{

			NugetDirectory.DeleteDirectory();

			string NuGetVersionCustom = "1.0.0";
			int commitNum = 0;
			if (Int32.TryParse(GitVersion.CommitsSinceVersionSource, out commitNum))
			{

			}

			NuGetVersionCustom = NuGetVersionCustom + "-" + "preview." + commitNum;

			//string outputPackagePath = NugetDirectory / ("HolyClient.SDK." + NuGetVersionCustom + ".nupkg");



			DotNetPack(s => s
			   .SetProject(Solution.GetProject("HolyClient.Abstractions"))
			   .SetConfiguration(Configuration)
			   .EnableNoBuild()
			   .EnableNoRestore()
			   .SetVersion(NuGetVersionCustom)
			   .SetNoDependencies(true)
			   .SetOutputDirectory(NugetDirectory));

			DotNetPack(s => s
			   .SetProject(Solution.GetProject("HolyClient.SDK"))
			   .SetConfiguration(Configuration)
			   .EnableNoBuild()
			   .EnableNoRestore()
			   .SetVersion(NuGetVersionCustom)
			   .SetNoDependencies(true)
			   .SetOutputDirectory(NugetDirectory));

			LocalNugetPath.CreateDirectory();

			NugetDirectory.GlobFiles("*.nupkg")
				  .NotEmpty()
				  // .Where(x => !x.EndsWith("symbols.nupkg"))
				  .ForEach(x =>
				  {
					  DotNetNuGetPush(s => s
						.SetTargetPath(x)
						  .SetSource(NugetApiUrl)
						  .SetApiKey(NugetApiKey)
					  );
				  });

		});


	
}
