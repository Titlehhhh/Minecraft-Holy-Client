/*
using System.Diagnostics;
using Humanizer;
using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

public class A
{
    #region MyRegion

    private void GenerateCore(NetNamespace netNamespace, Namespace clientPackets, Namespace serverPackets, string name)
    {
        var coreClass = new NetClass();

        coreClass.BaseClass = "ProtocolBase";
        coreClass.Name = $"Protocol_{Version}";

        coreClass.IsSealed = true;
        netNamespace.Classes.Add(coreClass);

        var constructor = new NetClass.NetConstructor();

        constructor.Arguments.Add(("IPacketBroker", "client"));
        constructor.BaseType = NetClass.NetConstructor.ConstructorBaseType.Base;
        constructor.Instructions.Add($"SupportedVersion = {Version};");
        coreClass.Constructors.Add(constructor);

        {
            var idMap = clientPackets.Types["packet"] as ProtodefContainer;
            var mapper = (ProtodefMapper)idMap.First(x => x.Name == "name").Type;
            var nameToId = mapper.Mappings
                .ToDictionary(id => "packet_" + id.Value, v => v.Key);


            foreach (var (packetName, type) in clientPackets.Types)
                if (packetName != "packet")
                {
                    var fields = type as ProtodefContainer;
                    if (fields.IsAllFieldsPrimitive())
                    {
                        var needContinue = false;

                        foreach (var f in fields)
                            if (f.Type is ProtodefCustomType cus)
                                if (!customToNet.ContainsKey(cus.Name))
                                {
                                    needContinue = true;
                                    break;
                                }

                        if (needContinue) continue;


                        var method = CreateSendMethod(fields, packetName, nameToId[packetName]);

                        coreClass.Methods.Add(method);
                    }
                }
        }

        {
            var recieveMethod = new NetMethod();

            recieveMethod.Modifier = "protected";
            recieveMethod.IsOverride = true;
            recieveMethod.Name = "OnPacketReceived";
            recieveMethod.ReturnType = "void";
            recieveMethod.Arguments.Add(("InputPacket", "packet"));

            var idMap = serverPackets.Types["packet"] as ProtodefContainer;
            var mapper = (ProtodefMapper)idMap.First(x => x.Name == "name").Type;
            var nameToId = mapper.Mappings
                .ToDictionary(id => "packet_" + id.Value, v => v.Key);

            coreClass.Methods.Add(recieveMethod);

            recieveMethod.Instructions.Add("switch(packet.Id)\n{");

            foreach (var (packetName, type) in serverPackets.Types)
                if (packetName != "packet")
                {
                    var clipPacketName = packetName.Substring("packet_".Length);
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

                    var fields = type as ProtodefContainer;
                    if (fields is null)
                        Debugger.Break();
                    if (fields.IsAllFieldsPrimitive())
                    {
                        var needContinue = false;

                        foreach (var f in fields)
                            if (f.Type is ProtodefCustomType cus)
                                if (!customToNet.ContainsKey(cus.Name))
                                {
                                    needContinue = true;
                                    break;
                                }

                        if (needContinue) continue;

                        try
                        {
                            var packetClass = CreateClassForPacket(fields, packetName);


                            var subjectField = new NetField();


                            subjectField.Name = $"_on{clipPacketName}";
                            subjectField.Modifier = "private";
                            subjectField.Type = $"Subject<{packetClass.Name}>";
                            subjectField.IsReadOnly = true;
                            subjectField.DefaultValue = "new ()";
                            coreClass.Fields.Add(subjectField);

                            var observableProp = new NetProperty();

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

            recieveMethod.Instructions.Add("}");
        }
    }


    private IEnumerable<string> GenerateReadInstructions(ProtodefContainer container, string subjectType,
        string packetClassName)
    {
        yield return $"if({subjectType}.HasObservers){{";
        yield return "scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);";
        List<string> vars = new();
        var number = 0;
        foreach (var field in container)
        {
            var fieldVar = field.Name.Camelize();
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
            var writeName = readDict[protodefNumeric.OriginalName];

            return $"{varName} = reader.{writeName}();";
        }

        if (type is ProtodefVarInt) return $"{varName} = reader.ReadVarInt();";

        if (type is ProtodefVarLong) return $"{varName} = reader.ReadVarLong();";

        if (type is ProtodefString str) return $"{varName} = reader.ReadString();";

        if (type is ProtodefVoid) return "";

        if (type is ProtodefBool) return $"{varName} = reader.ReadBoolean();";

        if (type is ProtodefOption option)
        {
            var index = varName.IndexOf(" ");
            var a = index != -1 ? varName.Substring(index) : varName;
            return $"{varName} = null;" +
                   $"if(reader.ReadBoolean())\n" +
                   $"{{\n" +
                   $"\t{GenerateReadMethod(option.Type, a, depth + 1)}\n" +
                   $"}}";
        }

        if (type is ProtodefArray array)
        {
            var iterator = $"i_{number}_{depth}";
            var tmpArrname = $"tempArray_{number}_{depth}";
            var lenName = $"tempArrayLength_{number}_{depth}";
            var len = "var " + lenName;
            var forItem = $"for_item_{number}_{depth}";
            var first = GenerateReadMethod(array.CountType, len, depth + 1);


            var netType = GetNetType(array.Type);
            return $"{first}\n" +
                   $"var {tmpArrname} = new {netType}[{lenName}];\n" +
                   $"for({GetNetType(array.CountType)} {iterator} =0;{iterator}< {lenName};{iterator}++)\n" +
                   $"{{\n" +
                   $"\t{GenerateReadMethod(array.Type, "var " + forItem, depth + 1)}\n" +
                   $"\t{tmpArrname}[{iterator}] = {forItem};\n" +
                   $"}}\n" +
                   $"{varName} = {tmpArrname};";
        }

        if (type is ProtodefBuffer buffer)
        {
            if (buffer.CountType is not null)
            {
                var lenName = $"tempArrayLength_{number}_{depth}";
                var len = "var " + lenName;
                var first = GenerateReadMethod(buffer.CountType, len, depth + 1);

                return $"{first}\n" +
                       $"{varName} = reader.ReadBuffer({lenName});";
            }

            if (buffer.Count is not null)
                return $"{varName} = reader.ReadBuffer({buffer.Count})";
            if (buffer.Rest == true)
                return $"{varName} = reader.ReadRestBuffer();";
            throw new Exception("Buffer fatal");
        }

        if (type is ProtodefCustomType cus)
        {
            var writeName = readDict[cus.Name];

            return $"{varName} = reader.{writeName}();";
        }

        throw new Exception("Not support type: " + type);
    }

    private NetClass CreateClassForPacket(ProtodefContainer container, string name)
    {
        NetClass result = new();
        result.Name = name.Pascalize();

        var constructor = new NetClass.NetConstructor();
        result.Constructors.Add(constructor);
        foreach (var field in container)
        {
            var netType = GetNetType(field.Type);

            var fieldNamePascalCase = field.Name.Pascalize();
            result.Properties.Add(new NetProperty
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
        var method = new NetMethod();


        //method.IsAsync = true;
        method.Name = "Send" + name.Substring("packet_".Length).Pascalize();


        List<(string, string)> arguments = new();

        var instructions = new List<string>();

        instructions.Add("scoped var writer = new MinecraftPrimitiveWriterSlim();");
        instructions.Add($"writer.WriteVarInt({id});");
        foreach (var field in container)
        {
            if (field.Anon == true)
                throw new NotSupportedException("Anon no support");

            var netType = GetNetType(field.Type);

            arguments.Add((netType, field.Name.Camelize()));

            var writeMethod = GenerateWriteMethod(field.Type, field.Name.Camelize());

            instructions.AddRange(writeMethod.Split("\n"));
        }


        instructions.Add("return base.SendPacketCore(writer.GetWrittenMemory());");


        method.ReturnType = "ValueTask";

        method.Arguments = arguments;
        method.Instructions = instructions;

        return method;
    }


    private string GenerateWriteMethod(ProtodefType type, string name, int depth = 0)
    {
        if (type is ProtodefNumericType protodefNumeric)
        {
            var writeName = writeDict[protodefNumeric.OriginalName];

            return $"writer.{writeName}({name});";
        }

        if (type is ProtodefVarInt) return $"writer.WriteVarInt({name});";

        if (type is ProtodefVarLong) return $"writer.WriteVarLong({name});";

        if (type is ProtodefString str) return $"writer.WriteString({name});";

        if (type is ProtodefVoid) return "";

        if (type is ProtodefBool) return $"writer.WriteBoolean({name});";

        if (type is ProtodefOption option)
            return $"if ({name} is null)\n" +
                   $"{{\n" +
                   $"\twriter.WriteBoolean(false);\n" +
                   $"}}\n" +
                   $"else\n" +
                   $"{{\n" +
                   $"\twriter.WriteBoolean(true);\n" +
                   $"\t{GenerateWriteMethod(option.Type, name, depth + 1)}\n" +
                   $"}}\n";

        if (type is ProtodefArray array)
        {
            var iterator = $"i_{depth}";

            return $"{GenerateWriteMethod(array.CountType, name + ".Length", 1 + depth)}\n" +
                   $"for (int {iterator} = 0; {iterator} < {name}.Length; {iterator}++)\n" +
                   $"{{\n" +
                   $"\tvar value_{depth} = {name}[{iterator}];\n" +
                   $"\t{GenerateWriteMethod(array.Type, "value_" + depth, depth + 1)}\n" +
                   $"}}";
        }

        if (type is ProtodefContainer container)
        {
            foreach (var field in container)
            {
                Console.WriteLine(field.Name);
            }
        }

        if (type is ProtodefBuffer buffer)
        {
            if (buffer.CountType is not null)
                return $"{GenerateWriteMethod(buffer.CountType, name + ".Length", 1 + depth)}\n" +
                       $"writer.WriteBuffer({name});";
            if (buffer.Count is not null)
                return $"writer.WriteVarInt({buffer.Count});\n" +
                       $"writer.WriteBuffer({name}.AsSpan(0, {buffer.Count}));";
            if (buffer.Rest == true)
                return $"writer.WriteRestBuffer({name});";
            throw new Exception("Buffer fatal");
        }

        if (type is ProtodefCustomType cus)
        {
            var writeName = writeDict[cus.Name];

            return $"writer.{writeName}({name});";
        }

        throw new Exception("Not support type: " + type);
    }

    private string GetNetType(ProtodefType type)
    {
        string? netType = null;
        if (type is ProtodefCustomType customType)
            netType = customToNet[customType.Name];
        else
            netType = type.GetNetType();

        if (netType == null) throw new TypeNotSupportedException(type.ToString());

        return netType;
    }

    #endregion
}*/