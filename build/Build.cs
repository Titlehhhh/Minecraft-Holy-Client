using NuGet.ContentModel;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Tools.OctoVersion;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using ParameterAttribute = Nuke.Common.ParameterAttribute;




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
	static AbsolutePath BuildDirectory => RootDirectory / ".buildArtifacts";
	static readonly string PackageContentType = "application/octet-stream";
	//static string ChangeLogFile => RootDirectory / "CHANGELOG.md";

	[Parameter]

	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
	AbsolutePath SourceDirectory => RootDirectory / "src";

	[Parameter]
	readonly string Runtime;


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
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
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


	public static string[] Runtimes = { "win-x64", "win-x86", "win-arm64", "linux-x64", "linux-arm", "linux-arm64", "osx-x64", "osx-arm64" };


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
			Console.WriteLine("Version: " + MinVer.Version);

			if (GitRepository.IsOnMasterBranch())
			{
				DotNetNuGetPush(s => s
						.SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
						.SetSource("https://api.nuget.org/v3/index.json")
						.SetApiKey(NuGetApiKey));
			}
			else
			{

				DotNetNuGetPush(s => s
							.SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
							.SetSource("https://f.feedz.io/holyclient/holyclient/nuget/index.json")
							.SetApiKey(FeedzApiKey));				
			}
		});



	Target PublishApp => _ => _
		.DependsOn(Clean, Restore)
		.Requires(() => Configuration.Equals(Configuration.Release))
		//.Triggers(CreateRelease)
		.Executes(() =>
		{

			var publishCombinations =
				from project in new[] { Solution.Platfroms.HolyClient_Desktop }
				from framework in project.GetTargetFrameworks()
				from runtime in Runtimes
				select new { project, framework, runtime };

			DotNetPublish(x => x
				.SetProject(Solution.Platfroms.HolyClient_Desktop)
				.SetConfiguration(Configuration)
				.SetPublishSingleFile(true)
				.SetProperty("DebugSymbols", "False")
				.SetProperty("DebugType", "None")
				.SetPublishReadyToRun(true)
				.EnableSelfContained()
				.SetOutput(BuildDirectory)
				.SetFramework("net8.0")
				.SetRuntime(Runtime));


			BuildDirectory.ZipTo(ArtifactsDirectory / $"HolyClient.Desktop.{Runtime}.zip");



		});



	Target CreateRelease => _ => _
	   .Description($"Creating release for the publishable version.")
	   .Requires(() => Configuration.Equals(Configuration.Release))
	   .Executes(async () =>
	   {


		   var credentials = new Credentials(GitHubActions.Token);

		   var gitHubClient = new GitHubClient(new ProductHeaderValue(nameof(NukeBuild)),
				new InMemoryCredentialStore(credentials));




		   var (owner, name) = (GitRepository.GetGitHubOwner(), GitRepository.GetGitHubName());



		   var releaseTag = MinVer.Version;
		   //var changeLogSectionEntries = ChangelogTasks.ExtractChangelogSectionNotes(ChangeLogFile);
		   //var latestChangeLog = changeLogSectionEntries
		   //   .Aggregate((c, n) => c + Environment.NewLine + n);







		   var newRelease = new NewRelease(releaseTag)
		   {
			   //TargetCommitish = ,
			   Draft = true,
			   Name = $"v{releaseTag}",
			   Prerelease = !string.IsNullOrEmpty(MinVer.MinVerPreRelease),
			   Body = "Preview release"
		   };

		   var createdRelease = await gitHubClient
									   .Repository
									   .Release.Create(owner, name, newRelease);
		   var tasks = ArtifactsDirectory
		   .GlobFiles("**/*")
		   .Select(async zip =>
		   {

			   Console.WriteLine("File upload:" + zip.ToString());
			   await using var artifactStream = File.OpenRead(zip);
			   var fileName = Path.GetFileName(zip);
			   var assetUpload = new ReleaseAssetUpload
			   {
				   FileName = fileName,
				   ContentType = PackageContentType,
				   RawData = artifactStream,
			   };
			   await gitHubClient.Repository.Release.UploadAsset(createdRelease, assetUpload);


		   });

		   await Task.WhenAll(tasks);


		   var release = await gitHubClient
					   .Repository
					   .Release
			   .Edit(owner, name, createdRelease.Id, new ReleaseUpdate { Draft = false });


	   });


}
