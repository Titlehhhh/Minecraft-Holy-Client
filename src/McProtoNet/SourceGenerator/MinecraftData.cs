using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using SourceGenerator.MCDataModels;
using SourceGenerator.ProtoDefTypes;

namespace SourceGenerator;

public static class MinecraftData
{
    public static async Task<ProtocolCollection> ReadAllAsync(string dataPath)
    {
        
        
        
        var dataPathsJson = Path.Combine(dataPath, "dataPaths.json");

        dataPathsJson = await File.ReadAllTextAsync(dataPathsJson);

        var paths = JsonSerializer.Deserialize<DataPaths>(dataPathsJson);


        var allVersions = JsonSerializer.Deserialize<ProtocolVersion[]>(
            await File.ReadAllTextAsync(
                Path.Combine(dataPath, "pc", "common", "protocolVersions.json")));


        ProtocolCollection collection = new();

        int[] skippedVersions = { 1073741839, 755, 402, 403,404, 480, 490, 477, 575, 573, 393, 734, 736, 735,709,351,710,401 };

        foreach (var item in paths.Pc)
        {
            var protocol_path = Path.Combine(dataPath, item.Value.Protocol, "protocol.json");
            var version_path = Path.Combine(dataPath, item.Value.Version, "version.json");

            if (File.Exists(version_path))
                if (File.Exists(protocol_path))
                {
                    var version_json = await File.ReadAllTextAsync(version_path);


                    var version = JsonSerializer.Deserialize<VersionInfo>(version_json);


                    if (version.Version >= 754 && !skippedVersions.Contains(version.Version))
                    {
                        var protocol_json = await File.ReadAllTextAsync(protocol_path);

                        ProtodefParser parser = new(protocol_json);

                        var protocol = parser.Parse();


                        collection.Add(version.Version, new Protocol
                        {
                            JsonPackets = protocol,
                            Version = version
                        });
                    }
                }
        }

        return collection;
    }
}