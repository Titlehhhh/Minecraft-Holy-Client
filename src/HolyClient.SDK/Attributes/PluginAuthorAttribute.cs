using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.SDK.Attributes
{
	public sealed class PluginAuthorAttribute : Attribute
	{
		public string Author { get; }

		public PluginAuthorAttribute(string author)
		{
			Author = author;
		}
	}
}
