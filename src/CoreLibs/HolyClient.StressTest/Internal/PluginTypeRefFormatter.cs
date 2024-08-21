using HolyClient.Core.Infrastructure;
using MessagePack;
using MessagePack.Formatters;

namespace HolyClient.StressTest;

public class PluginTypeRefFormatter : IMessagePackFormatter<PluginTypeReference>
{
    public static PluginTypeRefFormatter Instance { get; } = new();

    public PluginTypeReference Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil()) return default;

        options.Security.DepthStep(ref reader);

        var assembly = "";
        var fullname = "";

        var count = reader.ReadArrayHeader();
        for (var i = 0; i < count; i++)
            switch (i)
            {
                case 0:
                    assembly = reader.ReadString();
                    break;
                case 1:
                    fullname = reader.ReadString();
                    break;
                default:
                    reader.Skip();
                    break;
            }

        reader.Depth--;
        return new PluginTypeReference(assembly, fullname);
    }

    public void Serialize(ref MessagePackWriter writer, PluginTypeReference value, MessagePackSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(value.AssemblyName)
            || string.IsNullOrWhiteSpace(value.FullName))
        {
            writer.WriteNil();
            return;
        }

        writer.WriteArrayHeader(2);
        writer.Write(value.AssemblyName);
        writer.Write(value.FullName);
        writer.Flush();
    }
}