using System;
using MessagePack;

namespace HolyClient.AppState;

[MessagePackObject]
public struct BehaviorKey
{
    [Key(0)] public readonly string Name;
    [Key(1)] public readonly string Assembly;

    public BehaviorKey(string name, string assembly)
    {
        Name = name;
        Assembly = assembly;
    }


    public bool Equals(BehaviorKey c)
    {
        return c is BehaviorKey
               && (Name, Assembly)
               == (c.Name, c.Assembly);
    }

    public override bool Equals(object o)
    {
        return o is BehaviorKey c && Equals(c);
    }

    public static bool operator ==(in BehaviorKey c1, in BehaviorKey c2)
    {
        return Equals(c1, c2);
    }

    public static bool operator !=(in BehaviorKey c1, in BehaviorKey c2)
    {
        return !Equals(c1, c2);
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Assembly);
    }
}