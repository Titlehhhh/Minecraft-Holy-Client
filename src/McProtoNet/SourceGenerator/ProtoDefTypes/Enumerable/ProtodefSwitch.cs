using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes
{


	public class ProtodefSwitch : ProtodefType, IPathTypeEnumerable
	{




		//TODO path parser
		[JsonPropertyName("compareTo")]
		public string CompareTo { get; set; }

		[JsonPropertyName("compareToValue")]
		public string? CompareToValue { get; set; }

		[JsonPropertyName("fields")]
		public Dictionary<string, ProtodefType> Fields { get; set; } = new();

		[JsonPropertyName("default")]
		public ProtodefType? Default { get; set; }


		public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
		{
			foreach (var item in Fields)
			{
				yield return new KeyValuePair<string, ProtodefType>(item.Key, item.Value);
			}
			if (Default is IPathTypeEnumerable)
				yield return new("default", Default);
		}


	}


}
