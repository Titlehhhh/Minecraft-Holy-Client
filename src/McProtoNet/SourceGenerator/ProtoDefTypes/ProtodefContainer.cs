using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{

	public sealed class ProtodefContainer : ProtodefType, IEnumerable<ProtodefContainerField>, IFieldsEnumerable
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

		IEnumerator<KeyValuePair<string, ProtodefType>> IFieldsEnumerable.GetEnumerator()
		{

			foreach (var item in fields)
			{
				yield return new(item.Name, item.Type);
			}

		}
	}


}
