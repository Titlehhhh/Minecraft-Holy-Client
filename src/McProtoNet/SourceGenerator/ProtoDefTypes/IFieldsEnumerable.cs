namespace SourceGenerator.ProtoDefTypes
{
	public interface IFieldsEnumerable
	{
		IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator();
	}


}
