using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{

	public sealed class ProtodefContainer : ProtodefType, IEnumerable<ProtodefContainerField>
	{
		private readonly List<ProtodefContainerField> fields = new();

		[JsonConstructor]
		public ProtodefContainer(List<ProtodefContainerField> fields)
		{
			this.fields = fields;
		}

		public IEnumerator<ProtodefContainerField> GetEnumerator()
		{
			return fields.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return fields.GetEnumerator();
		}
	}


}
