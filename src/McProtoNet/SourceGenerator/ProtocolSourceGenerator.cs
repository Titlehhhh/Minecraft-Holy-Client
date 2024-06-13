using SourceGenerator.ProtoDefTypes;
using System.Diagnostics;
using System.Text;
using Humanizer;
using SourceGenerator.NetTypes;
using System;

public sealed class ProtocolSourceGenerator
{
	public ProtodefProtocol Protocol;
	public string Version;
	public int ProtocolVersion;





	public NetNamespace Generate()
	{
		NetNamespace netNamespace = new NetNamespace();
		netNamespace.Name = "McProtoNet.Protocol" + Version;

		netNamespace.Usings.Add("McProtoNet.Serialization");
		netNamespace.Usings.Add("McProtoNet.Protocol");

		foreach ((string nsName, Namespace side) in Protocol.Namespaces)
		{
			var serverPackets = side.Types["toClient"] as Namespace;
			var clientPackets = side.Types["toServer"] as Namespace;

			if (nsName == "play")
			{
				GenerateCore(netNamespace, clientPackets, serverPackets, nsName);
			}
		}

		return netNamespace;
	}


	private void GenerateCore(NetNamespace netNamespace, Namespace clientPackets, Namespace serverPackets, string name)
	{
		NetClass coreClass = new NetClass();

		coreClass.BaseClass = "ProtocolBase";
		coreClass.Name = $"Protocol_{Version}";
		coreClass.IsSealed = true;
		netNamespace.Classes.Add(coreClass);

		{
			ProtodefContainer idMap = clientPackets.Types["packet"] as ProtodefContainer;
			ProtodefMapper mapper = (ProtodefMapper)idMap.First(x => x.Name == "name").Type;
			Dictionary<string, string> nameToId = mapper.Mappings
				.ToDictionary(id => "packet_" + id.Value, v => v.Key);


			foreach ((string packetName, ProtodefType type) in clientPackets.Types)
			{
				if (packetName != "packet")
				{

					ProtodefContainer fields = type as ProtodefContainer;
					if (fields.IsAllFieldsPrimitive())
					{

						bool needContinue = false;

						foreach (var f in fields)
						{
							if (f.Type is ProtodefCustomType cus)
							{
								if (!customToNet.ContainsKey(cus.Name))
								{
									needContinue = true;
									break;
								}
							}
						}

						if (needContinue)
						{
							continue;
						}


						var method = CreateSendMethod(fields, packetName, nameToId[packetName]);

						coreClass.Methods.Add(method);
					}
				}
			}
		}


	}

	private NetMethod CreateSendMethod(ProtodefContainer container, string name, string id)
	{
		NetMethod method = new NetMethod();

		//method.IsAsync = true;
		method.Name = "Send" + name.Substring("packet_".Length).Pascalize();


		List<(string, string)> arguments = new();

		List<string> instructions = new List<string>();

		instructions.Add("scoped var writer = new MinecraftPrimitiveWriterSlim();");
		instructions.Add($"writer.WriteVarInt({id});");
		foreach (ProtodefContainerField field in container)
		{
			if (field.Anon == true)
				throw new NotSupportedException("Anon no support");

			string? netType = null;
			if (field.Type is ProtodefCustomType customType)
			{
				netType = customToNet[customType.Name];
			}
			else
			{
				netType = field.Type.GetNetType();
			}

			if (netType == null)
			{
				throw new Exception("Net type is empty");
			}


			arguments.Add((netType, field.Name));

			string writeMethod = GenerateWriteMethod(field.Type, field.Name);

			instructions.AddRange(writeMethod.Split("\n"));
		}


		instructions.Add("return base.SendPacketCore(writer.GetWrittenMemory());");


		method.ReturnType = "Task";

		method.Arguments = arguments;
		method.Instructions = instructions;

		return method;
	}

	private string GenerateWriteMethod(ProtodefType type, string name, int depth = 0)
	{
		if (type is ProtodefNumericType protodefNumeric)
		{
			string writeName = writeDict[protodefNumeric.OriginalName];

			return $"writer.{writeName}({name});";
		}
		else if (type is ProtodefVarInt)
		{
			return $"writer.WriteVarInt({name});";
		}
		else if (type is ProtodefVarLong)
		{
			return $"writer.WriteVarLong({name});";
		}
		else if (type is ProtodefString str)
		{
			return $"writer.WriteString({name});";
		}
		else if (type is ProtodefVoid)
		{
			return "";
		}
		else if (type is ProtodefBool)
		{
			return $"writer.WriteBoolean({name});";
		}
		else if (type is ProtodefOption option)
		{
			return $"if ({name} is null)\n" +
				$"{{\n" +
				$"\twriter.WriteBoolean(false);\n" +
				$"}}\n" +
				$"else\n" +
				$"{{\n" +
				$"\twriter.WriteBoolean(true);\n" +
				$"\t{GenerateWriteMethod(option.Type, name, depth + 1)}\n" +
				$"}}\n";
		}
		else if (type is ProtodefArray array)
		{

			string iterator = $"i_{depth}";

			return $"{GenerateWriteMethod(array.CountType, name + ".Length", 1 + depth)}\n" +
				$"for (int {iterator} = 0; {iterator} < {name}.Length; {iterator}++)\n" +
				$"{{\n" +
				$"\tvar value_{depth} = {name}[{iterator}];\n" +
				$"\t{GenerateWriteMethod(array.Type, "value_" + depth, depth + 1)}\n" +
				$"}}";
		}
		else if (type is ProtodefBuffer buffer)
		{
			if (buffer.CountType is not null)
			{
				return $"{GenerateWriteMethod(buffer.CountType, name + ".Length", 1 + depth)}\n" +
					$"writer.WriteBuffer({name});";
			}
			else if (buffer.Count is not null)
			{
				return $"writer.WriteVarInt({buffer.Count});\n" +
					$"writer.WriteBuffer({name});";
			}
			else if (buffer.Rest == true)
			{
				return $"writer.WriteRestBuffer({name});";
			}
			else
			{
				throw new Exception("Buffer fatal");
			}
		}
		else if (type is ProtodefCustomType cus)
		{
			string writeName = writeDict[cus.Name];

			return $"writer.{writeName}({name});";
		}
		else
		{
			throw new Exception("Not support type: " + type.ToString());
		}

	}


	private Dictionary<string, string> customToNet = new Dictionary<string, string>
	{
		{"UUID","Guid" },
		//{"position","Position" }
	};

	private Dictionary<string, string> readDict = new Dictionary<string, string>
	{
		{"varint","ReadVarInt" },
		{"varlong","ReadVarLong" },
		{"string","ReadString" },
		{"bool","ReadBoolean" },
		{"u8","ReadUnsignedByte" },
		{"i8","ReadSignedByte" },
		{"u16","ReadUnsignedShort" },
		{"i16","ReadSignedShort" },
		{"u32","ReadUnsignedInt" },
		{"i32","ReadSignedInt" },
		{"u64","ReadUnsignedLong" },
		{"i64","ReadSignedLong" },
		{"f32","ReadFloat" },
		{"f64","ReadDouble" },
		{"UUID","ReadUUID" },
		{"restBuffer","ReadRestBuffer" }
	};



	private Dictionary<string, string> writeDict = new Dictionary<string, string>
	{
		{"varint","WriteVarInt" },
		{"varlong","WriteVarLong" },
		{"string","WriteString" },
		{"bool","WriteBoolean" },
		{"u8","WriteUnsignedByte" },
		{"i8","WriteSignedByte" },
		{"u16","WriteUnsignedShort" },
		{"i16","WriteSignedShort" },
		{"u32","WriteUnsignedInt" },
		{"i32","WriteSignedInt" },
		{"u64","WriteUnsignedLong" },
		{"i64","WriteSignedLong" },
		{"f32","WriteFloat" },
		{"f64","WriteDouble" },
		{"UUID","WriteUUID" },
		{"restBuffer","WriteRestBuffer" },
	};

	public ProtocolSourceGenerator()
	{

	}

	private static string FieldToNetType(ProtodefContainerField field)
	{
		return field.Type.GetNetType();
	}

}




