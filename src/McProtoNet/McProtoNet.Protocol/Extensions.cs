using McProtoNet.Serialization;

namespace McProtoNet.Protocol;

public static class Extensions
{
    public static Position ReadPosition(this ref MinecraftPrimitiveReaderSlim reader)
    {
        var locEncoded = reader.ReadSignedLong();


        int x, y, z;
        //if (protocolversion >= Protocol18Handler.MC_1_14_Version)
        //{
        x = (int)(locEncoded >> 38);
        y = (int)(locEncoded & 4095);
        z = (int)((locEncoded << 26) >> 38);
        //}
        //else
        //{
        //	x = (int)(locEncoded >> 38);
        //	y = (int)((locEncoded >> 26) & 0xFFF);
        //	z = (int)(locEncoded << 38 >> 38);
        //}

        if (x >= 0x02000000) // 33,554,432
            x -= 0x04000000; // 67,108,864
        if (y >= 0x00000800) //      2,048
            y -= 0x00001000; //      4,096
        if (z >= 0x02000000) // 33,554,432
            z -= 0x04000000; // 67,108,864


        return new Position(x, z, y);
    }

    public static void WritePosition(this ref MinecraftPrimitiveWriterSlim writer, Position position)
    {
        ulong a = ((((ulong)position.X) & 0x3FFFFFF) << 38) |
                  ((((ulong)position.Z) & 0x3FFFFFF) << 12) |
                  (((ulong)position.Y) & 0xFFF);
       // var g = BitConverter.GetBytes(a);

       // Array.Reverse(g);
         writer.WriteUnsignedLong(a);
        //writer.WriteBuffer(g);
    }


    public static void WriteSlot(this ref MinecraftPrimitiveWriterSlim writer, Slot? slot)
    {
        if (slot is null)
        {
            writer.WriteBoolean(false);
        }
        else
        {
            writer.WriteBoolean(true);
            writer.WriteVarInt(slot.ItemId);
            writer.WriteSignedByte(slot.ItemCount);
            writer.WriteOptionalNbt(slot.Nbt);
        }
    }

    public static Slot? ReadSlot(this ref MinecraftPrimitiveReaderSlim reader)
    {
        if (reader.ReadBoolean())
            return new Slot(reader.ReadVarInt(), reader.ReadSignedByte(), reader.ReadOptionalNbt());

        return null;
    }
}