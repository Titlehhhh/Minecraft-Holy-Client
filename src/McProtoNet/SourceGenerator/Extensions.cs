using SourceGenerator.ProtoDefTypes;

public static class Extensions
{
	public static bool IsVarLong(this ProtodefType type)
	{
		return type is ProtodefVarLong;
	}
	public static bool IsVarInt(this ProtodefType type)
	{
		return type is ProtodefVarInt;
	}
	public static bool IsVariableNumber(this ProtodefType type)
	{
		return type.IsVarInt() || type.IsVarLong();
	}
	public static bool IsNumber(this ProtodefType type)
	{
		return type is ProtodefNumericType;
	}

	public static bool IsNumberOrVar(this ProtodefType type)
	{
		return type.IsNumber() || type.IsVariableNumber();
	}
	private static bool IsBool(this ProtodefType type) => type is ProtodefBool;
	private static bool IsString(this ProtodefType type) => type is ProtodefString;
	private static bool IsVoid(this ProtodefType type) => type is ProtodefVoid;
	public static bool IsCustom(this ProtodefType type)
	{
		return (type is ProtodefCustomType);
	}
	public static bool IsCustomSwitch(this ProtodefType type)
	{
		return type is ProtodefCustomSwitch;
	}
	private static bool IsPrimitive(this ProtodefSwitch @switch)
	{
		bool fields = @switch.Fields.All(x => x.Value.IsPrimitive() || x.Value.IsPrimitiveArray());
		bool @default = true;
		if (@switch.Default is not null)
		{
			bool a = @switch.Default.IsPrimitive();
			bool b = @switch.Default.IsPrimitiveArray();
			@default = a || b;
		}
		return fields && @default;
	}
	public static bool IsPrimitiveSwitch(this ProtodefType type)
	{
		if (type is ProtodefSwitch @switch)
		{
			return IsPrimitive(@switch);
		}
		return false;
	}
	public static bool IsPrimitive(this ProtodefType type)
	{
		return type.IsNumberOrVar() || type.IsCustom() || IsBool(type) || IsString(type) || IsVoid(type);
	}

	public static bool IsPrimitiveArray(this ProtodefType type)
	{
		if (type is ProtodefArray arr)
		{
			return arr.Type.IsPrimitive();
		}
		return false;
	}


	public static bool IsAllFieldsPrimitive(this ProtodefContainer container)
	{
		return container.All(x => x.Type.IsPrimitive()
		|| x.Type.IsPrimitiveArray()
		|| x.Type.IsPrimitiveSwitch());
	}

	public static bool IsArray(this ProtodefType type)
	{
		return type is ProtodefArray;
	}
	public static bool IsContainer(this ProtodefType type)
	{
		return type is ProtodefContainer;
	}
	public static bool IsSwitch(this ProtodefType type)
	{
		return type is ProtodefSwitch;
	}

	public static bool IsBuffer(this ProtodefType type)
	{
		return type is ProtodefBuffer;
	}

}