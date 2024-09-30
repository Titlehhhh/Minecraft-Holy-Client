using McProtoNet.Serialization;

namespace McProtoNet.Protocol;

public static class Extensions
{
    public static Position ReadPosition(this ref MinecraftPrimitiveSpanReader spanReader)
    {
        var locEncoded = spanReader.ReadSignedLong();


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

    public static void WritePosition(this scoped ref MinecraftPrimitiveSpanWriter spanWriter, Position position)
    {
        var a = (((ulong)position.X & 0x3FFFFFF) << 38) |
                (((ulong)position.Z & 0x3FFFFFF) << 12) |
                ((ulong)position.Y & 0xFFF);
        // var g = BitConverter.GetBytes(a);

        // Array.Reverse(g);
        spanWriter.WriteUnsignedLong(a);
        //writer.WriteBuffer(g);
    }


    public static void WriteSlot(this ref MinecraftPrimitiveSpanWriter spanWriter, Slot? slot)
    {
        if (slot is null)
        {
            spanWriter.WriteBoolean(false);
        }
        else
        {
            spanWriter.WriteBoolean(true);
            spanWriter.WriteVarInt(slot.ItemId);
            spanWriter.WriteSignedByte(slot.ItemCount);
            spanWriter.WriteOptionalNbt(slot.Nbt);
        }
    }

    public static Slot? ReadSlot(this ref MinecraftPrimitiveSpanReader spanReader)
    {
        if (spanReader.ReadBoolean())
            return new Slot(spanReader.ReadVarInt(), spanReader.ReadSignedByte(), spanReader.ReadOptionalNbt());

        return null;
    }
}