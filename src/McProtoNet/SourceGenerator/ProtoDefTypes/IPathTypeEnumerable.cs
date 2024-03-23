namespace SourceGenerator.ProtoDefTypes
{
	public interface IPathTypeEnumerable
	{
		IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator();
	}


}
