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
using LibGit2Sharp;




[GitHubActions("continuous",
GitHubActionsImage.UbuntuLatest,
AutoGenerate = false,
FetchDepth = 0,
	OnPushBranches = new[]
	{
		"development"

	},
	OnPullRequestBranches = new[]
	{
		"development"
	},
	InvokedTargets = new[]
	{
		nameof(Preview),
	},
	EnableGitHubToken = true,
	CacheKeyFiles = new[] { "**/global.json", "**/*.csproj" },
	CacheIncludePatterns = new[] { ".nuke/temp", "~/.nuget/packages" },
	CacheExcludePatterns = new string[0]

)]
class Build : NukeBuild
{


	public static int Main() => Execute<Build>(x => x.Preview);


	//[Parameter("MyGet Feed Url for Public Access of Pre Releases")]
	//readonly string MyGetNugetFeed;
	//[Parameter("MyGet Api Key"), Secret]
	//readonly string MyGetApiKey;

	//[Parameter("Nuget Feed Url for Public Access of Pre Releases")]
	//readonly string NugetFeed;
	//[Parameter("Nuget Api Key"), Secret]
	//readonly string NuGetApiKey;

	//[Parameter("Copyright Details")]
	//readonly string Copyright;

	//[Parameter("Artifacts Type")]
	//readonly string ArtifactsType;

	//[Parameter("Excluded Artifacts Type")]
	//readonly string ExcludedArtifactsType;

	//[GitVersion]
	//readonly GitVersion GitVersion;

	//[GitRepository]
	//readonly GitRepository GitRepository;

	[Solution(GenerateProjects = true)]
	readonly Solution Solution;

	static GitHubActions GitHubActions => GitHubActions.Instance;
	static AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

	static readonly string PackageContentType = "application/octet-stream";
	static string ChangeLogFile => RootDirectory / "CHANGELOG.md";

	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{

		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(x =>
				x.SetProjectFile(Solution.Platfroms.HolyClient_Desktop));
		});

	Target Compile => _ => _
		.DependsOn(Restore)

		.Executes(() =>
		{

		});


	readonly AbsolutePath SetupExe = ArtifactsDirectory / "bin" / "setup.exe";
	readonly AbsolutePath HolyClient_Application = ArtifactsDirectory / "bin" / "HolyClient.Desktop.application";
	readonly AbsolutePath ApplicationFiles = ArtifactsDirectory / "bin" / "Application Files";

	readonly AbsolutePath ClickOncePreview = RootDirectory / "ClickOnceArtifacts";



	Target Preview => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{

			DotNetBuild(x =>
				x.SetProjectFile(Solution.Platfroms.HolyClient_Desktop)
				.EnableNoRestore());

			

			//ClickOncePreview.CreateOrCleanDirectory();

			//string pathRep = Repository.Clone(
			//	"https://github.com/Titlehhhh/Minecraft-Holy-Client",
			//	ClickOncePreview,
			//	new CloneOptions()
			//	{

			//		BranchName = "deploy"
			//	});


			

			MSBuildTasks.MSBuild(s => s

				.SetTargetPath(Solution.Platfroms.HolyClient_Desktop)
				.SetTargets("publish")
				.SetProperty("PublishProfile", "ClickOnceProfile")
				.SetProperty("PublishDir", ArtifactsDirectory / "bin"));

			var outDir = ClickOncePreview / "preview";

			outDir.DeleteDirectory();

			SetupExe.MoveToDirectory(outDir);
			HolyClient_Application.MoveToDirectory(outDir);
			ApplicationFiles.MoveToDirectory(outDir);

			//PushToDeploy();

		});

	private void PushToDeploy()
	{
		try
		{

			using (var repo = new Repository(ClickOncePreview))
			{
				RepositoryStatus status = repo.RetrieveStatus();
				var filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
				foreach (var file in filePaths)
				{
					repo.Index.Add(file);
					repo.Index.Write();
				}
				var signature = new Signature("CI/CD", "email@email.com", DateTimeOffset.Now);

				repo.Commit($"Auto generated", signature, signature);

				var remote = repo.Network.Remotes["deploy"];
				var options = new PushOptions
				{
					CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
					{
						Username = GitHubActions.RepositoryOwner,
						Password = GitHubActions.Token
					}
				};

				var pushRefSpec = $"refs/heads/deploy";
				repo.Network.Push(remote, pushRefSpec, options); //Push changes to the remote repository

			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);

			Console.WriteLine("Error occured during pushing the changes!");
			Console.WriteLine("Please manually commit and push the changes!");
			throw;
		}
	}

}
