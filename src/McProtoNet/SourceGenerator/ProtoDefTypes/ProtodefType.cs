using SourceGenerator.ProtoDefTypes.Converters;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	[JsonConverter(typeof(DataTypeConverter))]
	public abstract class ProtodefType : IJsonOnDeserialized
	{
		public ProtodefType? Parent { get; set; }

		public virtual void OnDeserialized()
		{
			if (this is IPathTypeEnumerable enums)
			{
				foreach (var item in enums)
				{
					item.Value.Parent = this;
				}
			}
		}

		public virtual string? GetNetType() => null;


	}


}
