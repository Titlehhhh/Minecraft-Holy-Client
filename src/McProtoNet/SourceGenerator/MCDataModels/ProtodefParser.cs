using System.Text.Json;
using System.Text.Json.Serialization;
using SourceGenerator.ProtoDefTypes;

namespace SourceGenerator.MCDataModels
{
    public class ProtodefParser
    {
        private readonly string json;

        public ProtodefParser(string json)
        {
            this.json = json;
        }

        public Protocol Parse()
        {
            JsonSerializerOptions options = new()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            //options.Converters.Add(new DataTypeConverter());


            return JsonSerializer.Deserialize<Protocol>(json, options);
        }
    }
}
