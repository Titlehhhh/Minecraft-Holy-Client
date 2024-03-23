using SourceGenerator.ProtoDefTypes.Converters;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{
	[JsonConverter(typeof(DataTypeConverter))]
	public abstract class ProtodefType
	{
		public ProtodefType? Parent { get; set; }
	}


}
