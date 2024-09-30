using McProtoNet.Serialization;

namespace McProtoNet.Protocol;

public class CommandNode
{
    //public CommandNodeFlags Flags { get; }

    public int[] Children { get; }

    public int? RedirectNode { get; }

    public string? Name { get; }

    public static CommandNode Read(MinecraftPrimitiveSpanReader spanReader)
    {
        //var flags = new CommandNodeFlags(reader.ReadUnsignedByte());

        //int childrenCount = reader.ReadVarInt();

        //int[] children = new int[childrenCount];

        //for (int i = 0; i < childrenCount; i++)
        //{
        //	children[i] = reader.ReadVarInt();
        //}

        //int? redirectNode = null;

        //switch (flags.HasRedirectNode)
        //{
        //	case true:
        //		redirectNode = reader.ReadVarInt();
        //		break;
        //}


        throw new NotImplementedException();
        //return new CommandNode(flags, children, redirectNode);
    }
}