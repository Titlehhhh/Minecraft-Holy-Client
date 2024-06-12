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
						"void" => new ProtodefVoid(),
						"string" => new ProtodefString(),
						"bool" => new ProtodefBool(),
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
			const string i8 = "i8";
			const string u8 = "u8";
			const string i16 = "i16";
			const string u16 = "u16";
			const string li16 = "li16";
			const string lu16 = "lu16";
			const string i32 = "i32";
			const string u32 = "u32";
			const string li32 = "li32";
			const string lu32 = "lu32";
			const string i64 = "i64";
			const string u64 = "u64";
			const string li64 = "li64";
			const string lu64 = "lu64";
			const string f32 = "f32";
			const string lf32 = "lf32";
			const string f64 = "f64";
			const string lf64 = "lf64";


			return name switch
			{
				i8 => new ProtodefNumericType("sbyte", i8, true, ByteOrder.BigEndian),
				u8 => new ProtodefNumericType("byte", u8, false, ByteOrder.BigEndian),
				i16 => new ProtodefNumericType("short", i16, true, ByteOrder.BigEndian),
				u16 => new ProtodefNumericType("ushort", u16, false, ByteOrder.BigEndian),
				li16 => new ProtodefNumericType("short", li16, true, ByteOrder.LittleEndian),
				lu16 => new ProtodefNumericType("ushort", lu16, false, ByteOrder.LittleEndian),
				i32 => new ProtodefNumericType("int", i32, true, ByteOrder.BigEndian),
				u32 => new ProtodefNumericType("unit", u32, false, ByteOrder.BigEndian),
				li32 => new ProtodefNumericType("int", li32, true, ByteOrder.LittleEndian),
				lu32 => new ProtodefNumericType("uint", lu32, false, ByteOrder.LittleEndian),
				i64 => new ProtodefNumericType("long", i64, true, ByteOrder.BigEndian),
				u64 => new ProtodefNumericType("ulong", u64, false, ByteOrder.BigEndian),
				li64 => new ProtodefNumericType("long", li64, true, ByteOrder.LittleEndian),
				lu64 => new ProtodefNumericType("ulong", lu64, false, ByteOrder.LittleEndian),
				f32 => new ProtodefNumericType("float", f32, true, ByteOrder.BigEndian),
				lf32 => new ProtodefNumericType("float", lf32, true, ByteOrder.LittleEndian),
				f64 => new ProtodefNumericType("double", f64, true, ByteOrder.BigEndian),
				lf64 => new ProtodefNumericType("double", lf64, true, ByteOrder.LittleEndian),
				_ => null
			};
		}

		public override void Write(Utf8JsonWriter writer, ProtodefType value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
