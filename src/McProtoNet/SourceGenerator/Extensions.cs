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

	public static bool IsCustom(this ProtodefType type)
	{
		return type is ProtodefCustomType;
	}
	public static bool IsCustomSwitch(this ProtodefType type)
	{
		return type is ProtodefCustomSwitch;
	}

	public static bool IsPrimitive(this ProtodefType type)
	{
		return type.IsNumberOrVar() || type.IsCustom();
	}

	public static bool IsPrimitiveArray(this ProtodefType type)
	{
		if(type is ProtodefArray arr)
		{
			return arr.Type.IsPrimitive();
		}
		return false;
	}


	public static bool IsAllFieldsPrimitive(this ProtodefContainer container)
	{
		return container.All(x => x.IsPrimitive() || x.IsPrimitiveArray());
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