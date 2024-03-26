using System.Text.Json;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes.Converters
{
	public sealed class DataTypeConverter : JsonConverter<ProtodefType>
	{
		public override ProtodefType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				string name = reader.GetString();

				var number = GetNumber(name);

				if (number is null)
				{

					return name switch
					{
						"varint" => new ProtodefVarInt(),
						"varlong" => new ProtodefVarLong(),
						"void"=> new ProtodefVoid(),
						"string"=> new ProtodefString(),
						"bool"=> new ProtodefBool(),
						_ => new ProtodefCustomType(name)
					};
				}
				else
				{
					return number;
				}
			}
			else if (reader.TokenType == JsonTokenType.StartArray)
			{
				reader.Read();
				string name = reader.GetString();




				//else
				reader.Read();
				try
				{
					ProtodefType? result = name switch
					{
						"container" => new ProtodefContainer(JsonSerializer.Deserialize<List<ProtodefContainerField>>(ref reader, options)),
						"bitfield" => new ProtodefBitField(JsonSerializer.Deserialize<List<ProtodefBitFieldNode>>(ref reader, options)),
						"buffer" => JsonSerializer.Deserialize<ProtodefBuffer>(ref reader, options),
						"mapper" => JsonSerializer.Deserialize<ProtodefMapper>(ref reader, options),
						"array" => JsonSerializer.Deserialize<ProtodefArray>(ref reader, options),
						"option" => new ProtodefOption(JsonSerializer.Deserialize<ProtodefType>(ref reader, options)),
						"pstring" => JsonSerializer.Deserialize<ProtodefPrefixedString>(ref reader, options),
						"switch" => JsonSerializer.Deserialize<ProtodefSwitch>(ref reader, options),
						"topBitSetTerminatedArray" => JsonSerializer.Deserialize<ProtodefTopBitSetTerminatedArray>(ref reader, options),
						_ => ReadUnknownType(ref reader, options, name)
					};
					reader.Read();
					//while (reader.Read()) ;
					result.OnDeserialized();
					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine("name: " + name);
					Console.WriteLine("Error deserialize: " + ex);
					throw;
				}
			}
			else
			{
				throw new JsonException();
			}
		}

		private ProtodefType? ReadUnknownType(ref Utf8JsonReader reader, JsonSerializerOptions options, string name)
		{
			using var doc = JsonDocument.ParseValue(ref reader);
			var original = doc.RootElement.Clone();

			var obj = original.EnumerateObject();
			if (obj.Count() == 1)
			{
				var compareTo = obj.First(x => x.NameEquals("compareTo"));
				return new ProtodefCustomSwitch
				{
					Owner = name,
					CompareTo = compareTo.Value.GetString()
				};
			}

			var loop = doc.Deserialize<ProtodefLoop>(options);

			if (loop.Type is null)
			{
				throw new Exception("is not loop");
			}

			return loop;


			throw new NotSupportedException($"Unknown type: {name}");
		}

		private ProtodefNumericType? GetNumber(string name)
		{
			return name switch
			{
				"i8" => new ProtodefNumericType("sbyte", true, ByteOrder.BigEndian),
				"u8" => new ProtodefNumericType("byte", false, ByteOrder.BigEndian),
				"i16" => new ProtodefNumericType("short", true, ByteOrder.BigEndian),
				"u16" => new ProtodefNumericType("ushort", false, ByteOrder.BigEndian),
				"li16" => new ProtodefNumericType("short", true, ByteOrder.LittleEndian),
				"lu16" => new ProtodefNumericType("ushort", false, ByteOrder.LittleEndian),
				"i32" => new ProtodefNumericType("int", true, ByteOrder.BigEndian),
				"u32" => new ProtodefNumericType("unit", false, ByteOrder.BigEndian),
				"li32" => new ProtodefNumericType("int", true, ByteOrder.LittleEndian),
				"lu32" => new ProtodefNumericType("uint", false, ByteOrder.LittleEndian),
				"i64" => new ProtodefNumericType("long", true, ByteOrder.BigEndian),
				"u64" => new ProtodefNumericType("ulong", false, ByteOrder.BigEndian),
				"li64" => new ProtodefNumericType("long", true, ByteOrder.LittleEndian),
				"lu64" => new ProtodefNumericType("ulong", false, ByteOrder.LittleEndian),
				"f32" => new ProtodefNumericType("float", true, ByteOrder.BigEndian),
				"lf32" => new ProtodefNumericType("float", true, ByteOrder.LittleEndian),
				"f64" => new ProtodefNumericType("double", true, ByteOrder.BigEndian),
				"lf64" => new ProtodefNumericType("double", true, ByteOrder.LittleEndian),
				_ => null
			};
		}

		public override void Write(Utf8JsonWriter writer, ProtodefType value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
