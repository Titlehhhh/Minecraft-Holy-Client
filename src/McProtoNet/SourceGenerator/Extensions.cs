using SourceGenerator.ProtoDefTypes;

public static class Extensions
{




	public static bool IsPrimitive(this ProtodefType type)
	{
		if (type is ProtodefNumericType)
			return true;
		if (type is ProtodefVarInt)
			return true;

		if (type is ProtodefVarLong)
			return true;

		if (type is ProtodefCustomType customType)
		{
			return customType.Name == "bool" || customType.Name == "string";
		}

		return false;
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