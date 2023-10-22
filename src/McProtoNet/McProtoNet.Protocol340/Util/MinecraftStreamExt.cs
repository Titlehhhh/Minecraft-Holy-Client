using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Data.World.Chunk;

namespace McProtoNet.Protocol340.Util
{
    public static class MinecraftStreamExt
    {

        private static int POSITION_X_SIZE = 38;
        private static int POSITION_Y_SIZE = 26;
        private static int POSITION_Z_SIZE = 38;
        private static int POSITION_Y_SHIFT = 0xFFF;
        private static int POSITION_WRITE_SHIFT = 0x3FFFFFF;
        public static Vector3 ReadPosition(this IMinecraftPrimitiveReader reader)
        {
            long val = reader.ReadLong();

            int x = (int)(val >> POSITION_X_SIZE);
            int y = (int)((val >> POSITION_Y_SIZE) & POSITION_Y_SHIFT);
            int z = (int)((val << POSITION_Z_SIZE) >> POSITION_Z_SIZE);
            return new Vector3(x, y, z);
        }
        public static void WritePosition(this IMinecraftPrimitiveWriter writer, Vector3 point)
        {
            long x = (int)point.X & POSITION_WRITE_SHIFT;
            long y = (int)point.Y & POSITION_Y_SHIFT;
            long z = (int)point.Z & POSITION_WRITE_SHIFT;

            writer.WriteLong(x << POSITION_X_SIZE | y << POSITION_Y_SIZE | z);
        }

        /*
        public static void WriteItem(this IMinecraftPrimitiveWriter writer, ItemStack? item)
        {
            writer.WriteBoolean(item != null);
            if (item != null)
            {
                writer.WriteVarInt(item.Id);
                writer.WriteByte(item.Amount);
                writer.WriteNbt(item.Nbt, root: true);
            }
        }

        public static ItemStack? ReadItem(this IMinecraftPrimitiveReader reader)
        {
            bool present = reader.ReadBoolean();
            if (!present)
                return null;
            int item = reader.ReadVarInt();

            var amount = reader.ReadSignedByte();
            NbtCompound? nbt = null;

            nbt = reader.ReadNbt();



            return new ItemStack(item, amount, nbt);
        }
        */


        public static BlockState ReadBlockState(this IMinecraftPrimitiveReader reader)
        {
            int rawId = reader.ReadVarInt();
            return new BlockState((ushort)(rawId >> 4), (byte)(rawId & 0xF));
        }

        private static bool available(Stream stream)
        {
            return stream.Length != stream.Position;
        }

        public static List<EntityMetadata> ReadEntityMetadata(this IMinecraftPrimitiveReader reader)
        {
            List<EntityMetadata> ret = new List<EntityMetadata>();
            byte id;
            while ((id = reader.ReadUnsignedByte()) != 255)
            {
                int typeId = reader.ReadVarInt();
                MetadataType type = (MetadataType)typeId;
                object value = null;
                switch (type)
                {
                    case MetadataType.BYTE:
                        value = reader.ReadSignedByte();
                        break;
                    case MetadataType.INT:
                        value = reader.ReadVarInt();
                        break;
                    case MetadataType.FLOAT:
                        value = reader.ReadFloat();
                        break;
                    case MetadataType.STRING:
                        value = reader.ReadString();
                        break;
                    case MetadataType.CHAT:
                        value = reader.ReadString();
                        break;
                    case MetadataType.ITEM:
                        // value =
                        break;
                    case MetadataType.BOOLEAN:
                        value = reader.ReadBoolean();
                        break;
                    case MetadataType.ROTATION:
                        // value = readRotation(in);
                        break;
                    case MetadataType.POSITION:
                        //  value = readPosition(in);
                        break;
                    case MetadataType.OPTIONAL_POSITION:
                        bool positionPresent = reader.ReadBoolean();
                        if (positionPresent)
                        {
                            //  value = ReadPosition(in);
                        }

                        break;
                    case MetadataType.BLOCK_FACE:
                        //   value = MagicValues.key(BlockFace.class, in.readVarInt());
                        break;
                    case MetadataType.OPTIONAL_UUID:
                        bool uuidPresent = reader.ReadBoolean();
                        if (uuidPresent)
                        {
                            value = reader.ReadUUID();
                        }

                        break;
                    case MetadataType.BLOCK_STATE:
                        //value = readBlockState(in);
                        break;
                    case MetadataType.NBT_TAG:
                        value = reader.ReadOptionalNbt();
                        break;
                    default:
                        throw new Exception("Unknown metadata type id: " + typeId);
                }

                ret.Add(new EntityMetadata(id, type, value));
            }

            return ret;
        }
    }
}
