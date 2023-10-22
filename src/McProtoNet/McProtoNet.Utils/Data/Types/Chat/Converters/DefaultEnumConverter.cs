
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace McProtoNet.Utils;

public class DefaultEnumConverter : JsonConverter
{
    public static readonly Regex pattern = new(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
//public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum && typeToConvert == typeof(T);

//public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//{
//    var value = reader.GetString();

//    if (value.StartsWith("minecraft:"))
//        value = TrimResourceTag(value);

//    return Enum.TryParse(typeof(T), value.Replace("_", ""), true, out var result) ? (T)result : throw new InvalidOperationException($"Failed to deserialize: {value}");
//}
//public static string TrimResourceTag(string value, bool keepUnderscores = false)
//{
//    var values = value.Split(':');

//    var resourceLocationLength = values[0].Length + 1;

//    int length = value.Length - resourceLocationLength;

//    if (!keepUnderscores)
//        length -= value.Count(c => c == '_');

//    return string.Create(length, value, (span, source) =>
//    {
//        int sourceIndex = resourceLocationLength;
//        for (int i = 0; i < span.Length;)
//        {
//            char sourceChar = source[sourceIndex];

//            if (keepUnderscores)
//            {
//                span[i] = sourceChar;
//                i++;
//            }
//            else if (sourceChar != '_')
//            {
//                span[i] = sourceChar;
//                i++;
//            }
//            sourceIndex++;
//        }
//    });
//}
//public static string ToSnakeCase(string str) => string.Join("_", pattern.Matches(str)).ToLower();
//public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(ToSnakeCase(value.ToString()));

