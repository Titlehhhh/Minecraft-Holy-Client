using MessagePack;
using System;

namespace HolyClient.Models;

[MessagePackObject]
public struct BotPluginReference
{
	[Key(0)]
	public readonly string Name;
	[Key(1)]
	public readonly string Assembly;

	public BotPluginReference(string name, string assembly)
	{
		Name = name;
		Assembly = assembly;
	}


	public bool Equals(BotPluginReference c)
	=> c is BotPluginReference
	&& (Name, Assembly)
	== (c.Name, c.Assembly);

	public override bool Equals(object o)
	  => (o is BotPluginReference c) && Equals(c);

	public static bool operator ==(in BotPluginReference c1, in BotPluginReference c2)
	  => Equals(c1, c2);

	public static bool operator !=(in BotPluginReference c1, in BotPluginReference c2)
	  => !Equals(c1, c2);



	public override int GetHashCode()
	  => HashCode.Combine(Name, Assembly);

}



