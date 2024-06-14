using System.Collections.Generic;
using MessagePack;

namespace HolyClient.AppState;

[MessagePackObject]
public class ExtensionManagerState
{
    [Key(0)] public List<string> References { get; set; } = new();

    //[Key(0)]
    //public List<NugetPackageReference> NugetPackages { get; set; } = new();

    public void AddReference(string path)
    {
        References.Add(path);
    }

    public void RemoveReference(string path)
    {
        References.Remove(path);
    }


    //public void AddPackage(string id, NuGetVersion version)
    //{
    //	NugetPackages.Add(new NugetPackageReference()
    //	{
    //		Id = id,
    //		Version = version.ToString()
    //	});
    //}
    //public void RemovePackage(string id, NuGetVersion version)
    //{

    //}
}