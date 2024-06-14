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
        netNamespace.Usings.Add("McProtoNet.Abstractions");
        netNamespace.Usings.Add("System.Reactive.Subjects");

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

        var constructor = new NetClass.NetConstructor();

        constructor.Arguments.Add(("IPacketBroker", "client"));
        constructor.BaseType = NetClass.NetConstructor.ConstructorBaseType.Base;

        coreClass.Constructors.Add(constructor);

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

        {
            NetMethod recieveMethod = new NetMethod();

            recieveMethod.Modifier = "protected";
            recieveMethod.IsOverride = true;
            recieveMethod.Name = "OnPacketReceived";
            recieveMethod.ReturnType = "void";
            recieveMethod.Arguments.Add(("InputPacket", "packet"));

            ProtodefContainer idMap = serverPackets.Types["packet"] as ProtodefContainer;
            ProtodefMapper mapper = (ProtodefMapper)idMap.First(x => x.Name == "name").Type;
            Dictionary<string, string> nameToId = mapper.Mappings
                .ToDictionary(id => "packet_" + id.Value, v => v.Key);

            coreClass.Methods.Add(recieveMethod);

            recieveMethod.Instructions.Add($"switch(packet.Id)\n{{");

            foreach ((string packetName, ProtodefType type) in serverPackets.Types)
            {
                if (packetName != "packet")
                {
                    string clipPacketName = packetName.Substring("packet_".Length);
                    string packetId = null;
                    try
                    {
                        packetId = nameToId[packetName];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(netNamespace.Name);
                        throw;
                    }

                    ProtodefContainer fields = type as ProtodefContainer;
                    if (fields is null)
                        Debugger.Break();
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

                        try
                        {
                            NetClass packetClass = CreateClassForPacket(fields, packetName);


                            NetField subjectField = new NetField();


                            subjectField.Name = $"_on{clipPacketName}";
                            subjectField.Modifier = "private";
                            subjectField.Type = $"Subject<{packetClass.Name}>";
                            subjectField.IsReadOnly = true;
                            subjectField.DefaultValue = "new ()";
                            coreClass.Fields.Add(subjectField);

                            NetProperty observableProp = new NetProperty();

                            observableProp.Name = $"On{clipPacketName.Pascalize()}Packet";
                            observableProp.Type = $"IObservable<{packetClass.Name}>";
                            observableProp.GetSet = $"=>{subjectField.Name};";

                            coreClass.Properties.Add(observableProp);

                            recieveMethod.Instructions.Add($"case {packetId}:");
                            recieveMethod.Instructions.AddRange(GenerateReadInstructions(fields, subjectField.Name,
                                packetClass.Name));
                            recieveMethod.Instructions.Add("break;");


                            netNamespace.Classes.Add(packetClass);
                        }
                        catch (TypeNotSupportedException)
                        {
                        }
                    }
                }
            }

            recieveMethod.Instructions.Add("}");
        }
    }


    private IEnumerable<string> GenerateReadInstructions(ProtodefContainer container, string subjectType,
        string packetClassName)
    {
        yield return $"if({subjectType}.HasObservers){{";
        yield return $"scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);";
        List<string> vars = new();
        int number = 0;
        foreach (var field in container)
        {
            string fieldVar = field.Name.Camelize();
            vars.Add(fieldVar);
            yield return GenerateReadMethod(field.Type, $"{GetNetType(field.Type)} {fieldVar}", depth: 0,
                number: number++);
        }

        yield return $"{subjectType}.OnNext(new {packetClassName}({string.Join(", ", vars)}));";
        yield return "}";
    }

    private string GenerateReadMethod(ProtodefType type, string varName, int number, int depth = 0)
    {
        if (type is ProtodefNumericType protodefNumeric)
        {
            string writeName = readDict[protodefNumeric.OriginalName];

            return $"{varName} = reader.{writeName}();";
        }
        else if (type is ProtodefVarInt)
        {
            return $"{varName} = reader.ReadVarInt();";
        }
        else if (type is ProtodefVarLong)
        {
            return $"{varName} = reader.ReadVarLong();";
        }
        else if (type is ProtodefString str)
        {
            return $"{varName} = reader.ReadString();";
        }
        else if (type is ProtodefVoid)
        {
            return "";
        }
        else if (type is ProtodefBool)
        {
            return $"{varName} = reader.ReadBoolean();";
        }
        else if (type is ProtodefOption option)
        {
            int index = varName.IndexOf(" ");
            string a = index != -1 ? varName.Substring(index) : varName;
            return $"{varName} = null;" +
                   $"if(reader.ReadBoolean())\n" +
                   $"{{\n" +
                   $"\t{GenerateReadMethod(option.Type, a, depth + 1)}\n" +
                   $"}}";
        }
        else if (type is ProtodefArray array)
        {
            string iterator = $"i_{number}_{depth}";
            string tmpArrname = $"tempArray_{number}_{depth}";
            string lenName = $"tempArrayLength_{number}_{depth}";
            string len = "var " + lenName;
            string forItem = $"for_item_{number}_{depth}";
            string first = GenerateReadMethod(array.CountType, len, depth + 1);


            string netType = GetNetType(array.Type);
            return $"{first}\n" +
                   $"var {tmpArrname} = new {netType}[{lenName}];\n" +
                   $"for({GetNetType(array.CountType)} {iterator} =0;{iterator}< {lenName};{iterator}++)\n" +
                   $"{{\n" +
                   $"\t{GenerateReadMethod(array.Type, "var " + forItem, depth + 1)}\n" +
                   $"\t{tmpArrname}[{iterator}] = {forItem};\n" +
                   $"}}\n" +
                   $"{varName} = {tmpArrname};";
        }
        else if (type is ProtodefBuffer buffer)
        {
            if (buffer.CountType is not null)
            {
                string lenName = $"tempArrayLength_{number}_{depth}";
                string len = "var " + lenName;
                string first = GenerateReadMethod(buffer.CountType, len, depth + 1);

                return $"{first}\n" +
                       $"{varName} = reader.ReadBuffer({lenName});";
            }
            else if (buffer.Count is not null)
            {
                return $"{varName} = reader.ReadBuffer({buffer.Count})";
            }
            else if (buffer.Rest == true)
            {
                return $"{varName} = reader.ReadRestBuffer();";
            }
            else
            {
                throw new Exception("Buffer fatal");
            }
        }
        else if (type is ProtodefCustomType cus)
        {
            string writeName = readDict[cus.Name];

            return $"{varName} = reader.{writeName}();";
        }
        else
        {
            throw new Exception("Not support type: " + type.ToString());
        }
    }

    private NetClass CreateClassForPacket(ProtodefContainer container, string name)
    {
        NetClass @result = new();
        result.Name = name.Pascalize();

        NetClass.NetConstructor constructor = new NetClass.NetConstructor();
        result.Constructors.Add(constructor);
        foreach (var field in container)
        {
            string netType = GetNetType(field.Type);

            string fieldNamePascalCase = field.Name.Pascalize();
            result.Properties.Add(new NetProperty()
            {
                GetSet = "{ get; internal set; }",
                Modifier = "public",
                Name = fieldNamePascalCase,
                Type = netType
            });

            constructor.Arguments.Add((netType, field.Name));

            constructor.Instructions.Add($"{fieldNamePascalCase} = {field.Name};");
        }

        return result;
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

            string netType = GetNetType(field.Type);

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
                       $"writer.WriteBuffer({name}.AsSpan(0, {buffer.Count}));";
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

    private string GetNetType(ProtodefType type)
    {
        string? netType = null;
        if (type is ProtodefCustomType customType)
        {
            netType = customToNet[customType.Name];
        }
        else
        {
            netType = type.GetNetType();
        }

        if (netType == null)
        {
            throw new TypeNotSupportedException(type.ToString());
        }

        return netType;
    }

    private Dictionary<string, string> customToNet = new Dictionary<string, string>
    {
        { "UUID", "Guid" },
        {"position","Position" },
        {"slot","Slot?" },
        {"restBuffer","byte[]" },
    };

    private Dictionary<string, string> readDict = new Dictionary<string, string>
    {
        { "varint", "ReadVarInt" },
        { "varlong", "ReadVarLong" },
        { "string", "ReadString" },
        { "bool", "ReadBoolean" },
        { "u8", "ReadUnsignedByte" },
        { "i8", "ReadSignedByte" },
        { "u16", "ReadUnsignedShort" },
        { "i16", "ReadSignedShort" },
        { "u32", "ReadUnsignedInt" },
        { "i32", "ReadSignedInt" },
        { "u64", "ReadUnsignedLong" },
        { "i64", "ReadSignedLong" },
        { "f32", "ReadFloat" },
        { "f64", "ReadDouble" },
        { "UUID", "ReadUUID" },
        { "restBuffer", "ReadRestBuffer" },
        { "position", "ReadPosition" },
        { "slot", "ReadSlot" },
       
    };


    private Dictionary<string, string> writeDict = new Dictionary<string, string>
    {
        { "varint", "WriteVarInt" },
        { "varlong", "WriteVarLong" },
        { "string", "WriteString" },
        { "bool", "WriteBoolean" },
        { "u8", "WriteUnsignedByte" },
        { "i8", "WriteSignedByte" },
        { "u16", "WriteUnsignedShort" },
        { "i16", "WriteSignedShort" },
        { "u32", "WriteUnsignedInt" },
        { "i32", "WriteSignedInt" },
        { "u64", "WriteUnsignedLong" },
        { "i64", "WriteSignedLong" },
        { "f32", "WriteFloat" },
        { "f64", "WriteDouble" },
        { "UUID", "WriteUUID" },
        { "restBuffer", "WriteBuffer" },
        { "position", "WritePosition" },
        { "slot", "WriteSlot" },
        
    };

    public ProtocolSourceGenerator()
    {
    }

    private static string FieldToNetType(ProtodefContainerField field)
    {
        return field.Type.GetNetType();
    }
}