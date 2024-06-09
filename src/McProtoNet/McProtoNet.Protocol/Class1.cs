using McProtoNet.NBT;
using McProtoNet.Serialization;
using System.Collections.ObjectModel;

namespace McProtoNet.Protocol
{
	public sealed class Slot
	{
		public int ItemId { get; }
		public short ItemCount { get; }
		public NbtCompound? Nbt { get; }
	}

	public sealed class Particle
	{
		public int Id { get; }

		public ParticleData Data { get; }
		public class ParticleData
		{
			//TODO
		}
	}
	public sealed class Ingredient : Collection<Slot>
	{

	}

	public struct Position
	{
		public int X { get; }

		public int Y { get; }

		public int Z { get; }

		public Position(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}

	public sealed class EntityMetadataItem
	{

	}

	public sealed class EntityMetadata
	{

	}

	public class MinecraftSmeltingFormat
	{
		public string Group { get; private set; }
		public Ingredient Ingredient { get; private set; }
		public Slot Result { get; private set; }
		public float Experience { get; private set; }
		public int CookTime { get; private set; }
	}

	public sealed class Tag
	{
		public string TagName { get; private set; }
		public int[] Entries { get; private set; }
	}

	public class CommandNode
	{

		public static CommandNode Read(IMinecraftPrimitiveReader reader)
		{
			var flags = new CommandNodeBitField(reader.ReadUnsignedByte());

			int childrenCount = reader.ReadVarInt();

			int[] children = new int[childrenCount];

			for (int i = 0; i < childrenCount; i++)
			{
				children[i] = reader.ReadVarInt();
			}

			object redirectNode = null;

			switch (flags.HasRedirectNode)
			{
				case true:
					redirectNode = reader.ReadVarInt();
					break;
			}


			

			return new CommandNode(flags, children, redirectNode);
		}

		public CommandNode(CommandNodeBitField flags, int[] children, object redirectNode, object extraNodeData)
		{
			Flags = flags;
			Children = children;
			RedirectNode = redirectNode;
			ExtraNodeData = extraNodeData;
		}

		public CommandNodeBitField Flags { get; }

		public int[] Children { get; }

		public object RedirectNode { get; }

		public object ExtraNodeData { get; }
	}

	public struct CommandNodeBitField
	{
		public CommandNodeBitField(byte value)
		{

		}


		public const int Size = 8;


		public readonly ushort Unused;
		public readonly bool HasCustomSuggestions;
		public readonly bool HasRedirectNode;
		public readonly bool HasCommand;
		public readonly byte CommandNodeType;
	}
}
