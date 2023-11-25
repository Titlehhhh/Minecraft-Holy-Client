using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.CI.GitHubActions;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using System.IO;
using System.Threading.Tasks;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.ChangeLog;
using System;
using ParameterAttribute = Nuke.Common.ParameterAttribute;
using Microsoft.Build.Tasks;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities;
using Nuke.Common.Tools.Git;
using Serilog;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Tools.OctoVersion;
using Nuke.Common.Tools.MinVer;
using Octokit.Internal;
using Octokit;



class Build : NukeBuild
{


	public static int Main() => Execute<Build>(x => x.Print);


	[GitRepository] readonly GitRepository GitRepository;
	

	[Parameter] readonly string NuGetApiKey;

	[Parameter] readonly string FeedzApiKey;

	[Solution(GenerateProjects = true)]
	readonly Solution Solution;

	static GitHubActions GitHubActions => GitHubActions.Instance;
	static AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

	static readonly string PackageContentType = "application/octet-stream";
	static string ChangeLogFile => RootDirectory / "CHANGELOG.md";

	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	AbsolutePath SourceDirectory => RootDirectory / "src";

	

	[MinVer]
	readonly MinVer MinVer;

	Target Print => _ => _
		.Executes(() =>
		{
			Console.WriteLine(MinVer.ToJson(new Newtonsoft.Json.JsonSerializerSettings
			{
				Formatting = Newtonsoft.Json.Formatting.Indented
			}));
		});

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x=>x.DeleteDirectory());
			//TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
			ArtifactsDirectory.CreateOrCleanDirectory();
		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(x =>
				x.SetProjectFile(Solution));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Requires(() => Configuration.Equals(Configuration.Release))
		.Executes(() =>
		{
			//Build QuickProxy
			DotNetBuild(x =>
				x.SetProjectFile(Solution.ProxyLib.QuickProxyNet)
				.SetAssemblyVersion(MinVer.AssemblyVersion)
				.SetFileVersion(MinVer.FileVersion)
				.SetConfiguration(Configuration)
				.EnableNoRestore());

			//Build McProtoNet
			DotNetBuild(x =>
				x.SetProjectFile(Solution.McProtoNet.McProtoNet)
				.SetAssemblyVersion(MinVer.AssemblyVersion)
				.SetFileVersion(MinVer.FileVersion)
				.SetConfiguration(Configuration)
				.EnableNoRestore());

			//Build SDK

			DotNetBuild(x =>
				x.SetProjectFile(Solution.CoreLibs.HolyClient_Abstractions)
				.SetAssemblyVersion(MinVer.AssemblyVersion)
				.SetFileVersion(MinVer.FileVersion)
				.SetConfiguration(Configuration)
				.EnableNoRestore());

			DotNetBuild(x =>
				x.SetProjectFile(Solution.CoreLibs.HolyClient_SDK)
				.SetAssemblyVersion(MinVer.AssemblyVersion)
				.SetFileVersion(MinVer.FileVersion)
				
				.SetConfiguration(Configuration)
				.EnableNoRestore());

			//Build HolyClient.Desktop



			DotNetBuild(x =>
				x.SetProjectFile(Solution.Platfroms.HolyClient_Desktop)
				.SetAssemblyVersion(MinVer.AssemblyVersion)
				.SetFileVersion(MinVer.FileVersion)
				.SetConfiguration(Configuration)
				.EnableNoRestore());



		});


	readonly AbsolutePath SetupExe = ArtifactsDirectory / "setup.exe";
	readonly AbsolutePath HolyClient_Application = ArtifactsDirectory / "HolyClient.Desktop.application";
	readonly AbsolutePath ApplicationFiles = ArtifactsDirectory / "Application Files";

	readonly AbsolutePath NuGetDirectory = RootDirectory / "NuGetArtifacts";


	readonly AbsolutePath ClickOnceArtifacts = RootDirectory / "ClickOnceArtifacts";



	

	Target Pack => _ => _
		 .DependsOn(Clean, Compile)
		.Requires(() => Configuration.Equals(Configuration.Release))
		.Executes(() =>
		{

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				
				.SetProject(Solution.ProxyLib.QuickProxyNet)
				.SetOutputDirectory(ArtifactsDirectory));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.McProtoNet.McProtoNet_NBT)
				.SetOutputDirectory(ArtifactsDirectory));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.McProtoNet.McProtoNet_Utils)
				.SetOutputDirectory(ArtifactsDirectory));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.McProtoNet.McProtoNet_Core)
				.SetOutputDirectory(ArtifactsDirectory));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.McProtoNet.McProtoNet)
				.SetOutputDirectory(ArtifactsDirectory));



			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.CoreLibs.HolyClient_Abstractions)
				.SetOutputDirectory(ArtifactsDirectory));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetConfiguration(Configuration)
				.SetProject(Solution.CoreLibs.HolyClient_SDK)
				.SetOutputDirectory(ArtifactsDirectory));




		});



	Target LibsPush => _ => _
		.DependsOn(Pack)
		.Requires(() => Configuration.Equals(Configuration.Release))
		.Executes(() =>
		{
			DotNetNuGetPush(s => s						
						.SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
						.SetSource("https://f.feedz.io/holyclient/holyclient/nuget/index.json")
						.SetApiKey(FeedzApiKey)
					);
		});



	Target PublishApp => _ => _
		.DependsOn(Pack)
		.Requires(() => Configuration.Equals(Configuration.Release))
		.Produces(ArtifactsDirectory / "*.exe")
		.Executes(() =>
		{



			

			DotNetPublish(x => x
				.SetProject(Solution.Platfroms.HolyClient_Desktop)
				.EnableNoRestore()
				.SetConfiguration(Configuration)	
				.EnableNoBuild()
				.SetPublishProfile("FolderProfile")
				.SetProperty("PublishDir", ArtifactsDirectory));


		});



	Target CreateRelease => _ => _
	   .Description($"Creating release for the publishable version.")
	   .Requires(() => Configuration.Equals(Configuration.Release))
	   .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch() || GitRepository.IsOnReleaseBranch())
	   .Executes(async () =>
	   {
		   var credentials = new Credentials(GitHubActions.Token);
		   GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(nameof(NukeBuild)),
			   new InMemoryCredentialStore(credentials));

		   var (owner, name) = (GitRepository.GetGitHubOwner(), GitRepository.GetGitHubName());

		   var releaseTag = MinVer.Version;
		   var changeLogSectionEntries = ChangelogTasks.ExtractChangelogSectionNotes(ChangeLogFile);
		   var latestChangeLog = changeLogSectionEntries
			   .Aggregate((c, n) => c + Environment.NewLine + n);

		   var newRelease = new NewRelease(releaseTag)
		   {
			   //TargetCommitish = ,
			   Draft = true,
			   Name = $"v{releaseTag}",
			   Prerelease = !string.IsNullOrEmpty(MinVer.MinVerPreRelease),
			   Body = "Test Release"
		   };

		   var createdRelease = await GitHubTasks
									   .GitHubClient
									   .Repository
									   .Release.Create(owner, name, newRelease);

		   //   GlobFiles(ArtifactsDirectory, ArtifactsType)
		   //	  .Where(x => !x.EndsWith(ExcludedArtifactsType))
		   //	  .ForEach(async x => );

		   await UploadReleaseAssetToGithub(createdRelease, ArtifactsDirectory / "HolyClient.Desktop.exe");

		   await GitHubTasks
					  .GitHubClient
					  .Repository
					  .Release
			  .Edit(owner, name, createdRelease.Id, new ReleaseUpdate { Draft = false });
	   });


	private static async Task UploadReleaseAssetToGithub(Release release, string asset)
	{
		await using var artifactStream = File.OpenRead(asset);
		var fileName = Path.GetFileName(asset);
		var assetUpload = new ReleaseAssetUpload
		{
			FileName = fileName,
			ContentType = PackageContentType,
			RawData = artifactStream,
		};
		await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, assetUpload);
	}
}
