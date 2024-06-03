using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceGenerator.ProtoDefTypes.Attributes
{
	public sealed class NetTypeAttribute : Attribute
	{
		public string Name { get; }

		public NetTypeAttribute(string name)
		{
			Name = name;
		}
	}
}
