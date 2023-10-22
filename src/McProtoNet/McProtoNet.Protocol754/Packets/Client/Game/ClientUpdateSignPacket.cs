namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientUpdateSignPacket : MinecraftPacket
    {
        public Vector3 Position { get; private set; }
        public string[] Lines { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WritePosition(Position);
            foreach (var line in Lines)
            {
                stream.WriteString(line);
            }
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientUpdateSignPacket(Vector3 position, string[] lines)
        {
            Position = position;
            Lines = new string[lines.Length];

            Array.Copy(lines, Lines, lines.Length);
        }

        public ClientUpdateSignPacket() { }


    }
}
