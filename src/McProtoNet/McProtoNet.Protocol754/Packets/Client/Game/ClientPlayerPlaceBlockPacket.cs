namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientPlayerPlaceBlockPacket : MinecraftPacket
    {
        /*
         * out.writeVarInt(MagicValues.value(Integer.class, this.hand));
        Position.write(out, this.position);
        out.writeVarInt(MagicValues.value(Integer.class, this.face));
        out.writeFloat(this.cursorX);
        out.writeFloat(this.cursorY);
        out.writeFloat(this.cursorZ);
        out.writeBoolean(this.insideBlock);
         */
        public Hand Hand { get; private set; }
        public Vector3 Position { get; private set; }
        public BlockFace Face { get; private set; }
        public float CursorX { get; private set; }
        public float CursorY { get; private set; }
        public float CursorZ { get; private set; }
        public bool InsideBlock { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Hand);
            stream.WritePosition(Position);
            stream.WriteFloat(CursorX);
            stream.WriteFloat(CursorY);
            stream.WriteFloat(CursorZ);
            stream.WriteBoolean(InsideBlock);

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientPlayerPlaceBlockPacket(Hand hand, Vector3 position, BlockFace face, float cursorX, float cursorY, float cursorZ, bool insideBlock)
        {
            Hand = hand;
            Position = position;
            Face = face;
            CursorX = cursorX;
            CursorY = cursorY;
            CursorZ = cursorZ;
            InsideBlock = insideBlock;
        }

        public ClientPlayerPlaceBlockPacket() { }


    }
}
