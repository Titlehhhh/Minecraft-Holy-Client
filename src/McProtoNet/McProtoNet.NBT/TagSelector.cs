namespace McProtoNet.NBT
{
	/// <summary>
	/// Delegate used to skip loading certain tags of an NBT stream/file. 
	/// The callback should return "true" for any tag that should be read,and "false" for any tag that should be skipped.
	/// </summary>
	/// <param name="tag"> Tag that is being read. Tag's type and name are available,
	/// but the value has not yet been read at this time. Guaranteed to never be <c>null</c>. </param>
	// ReSharper disable once InconsistentNaming
	public delegate bool TagSelector(NbtTag tag);
}