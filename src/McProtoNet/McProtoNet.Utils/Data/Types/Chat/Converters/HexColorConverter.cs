


using Newtonsoft.Json;

namespace McProtoNet.Utils;

public class HexColorConverter : JsonConverter<HexColor>
{
	public override HexColor ReadJson(JsonReader reader, Type objectType, HexColor existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		string? value = reader.ReadAsString();
		if (value is not null)
			return new HexColor();
		return HexColor.White;
	}

	public override void WriteJson(JsonWriter writer, HexColor value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
		writer.Flush();
	}
}
