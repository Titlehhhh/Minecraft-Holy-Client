using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using ParameterAttribute = Nuke.Common.ParameterAttribute;

class Build : NukeBuild
{
    public static string[] Runtimes =
        { "win-x64", "win-x86", "win-arm64", "linux-x64", "linux-arm", "linux-arm64", "osx-x64", "osx-arm64" };

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Solution(GenerateProjects = true)] readonly Solution Solution;
    
    [MinVer] readonly MinVer MinVer;
    
    [Parameter] readonly string Runtime;
    
    Target Restore => _ => _
        .Executes(() =>
        {
           
            DotNetRestore(x =>
                x.SetProjectFile(Solution));
        });
    
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });
    static AbsolutePath BuildDirectory => RootDirectory / ".buildArtifacts";
    static GitHubActions GitHubActions => GitHubActions.Instance;
    
    Target DevBuild => _ => _
        .DependsOn(Restore)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            


        });

}
