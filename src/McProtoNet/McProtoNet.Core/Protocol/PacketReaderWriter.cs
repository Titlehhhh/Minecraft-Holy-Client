using McProtoNet.Core.IO;
using McProtoNet.Core.Packets;
using Microsoft.IO;

namespace McProtoNet.Core.Protocol
{
    public sealed class PacketReaderWriter : IPacketReaderWriter
    {
        private MinecraftStream minecraftStream;
        private IMinecraftProtocol minecraftProtocol;
        private IPacketProvider packets;
        private bool _disposeProtocol;

        public PacketReaderWriter(IMinecraftProtocol minecraftProtocol, IPacketProvider packets, bool disposeProtocol, MinecraftStream minecraftStream)
        {
            this.minecraftProtocol = minecraftProtocol;
            this.packets = packets;
            _disposeProtocol = disposeProtocol;
            this.minecraftStream = minecraftStream;
        }



        public async Task<MinecraftPacket> ReadNextPacketAsync(CancellationToken cancellationToken = default)
        {
            Packet readData = await minecraftProtocol.ReadNextPacketAsync(cancellationToken);
            using (readData.Data)
            {
                if (packets.TryGetInputPacket(readData.Id, out IInputPacket packet))
                {
                    MinecraftPrimitiveReader reader = new MinecraftPrimitiveReader();
                    reader.BaseStream = readData.Data;

                    packet.Read(reader);
                    return (MinecraftPacket)packet;
                }
            }
            throw new InvalidOperationException($"Input Packet {readData.Id} notFound");
        }


        public MinecraftPacket ReadNextPacket()
        {
            Packet readData = minecraftProtocol.ReadNextPacket();
            if (packets.TryGetInputPacket(readData.Id, out IInputPacket packet))
            {
                MinecraftPrimitiveReader reader = new MinecraftPrimitiveReader();
                reader.BaseStream = readData.Data;

                packet.Read(reader);
                return (MinecraftPacket)packet;
            }
            else
            {
                readData.Data.Dispose();
                throw new InvalidOperationException($"Input Packet {readData.Id} notFound");
            }
        }

        static RecyclableMemoryStreamManager streamManager = new();
        public async Task SendPacketAsync(MinecraftPacket packet, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool ok = packets.TryGetOutputId(packet, out int id);

            if (ok)
            {
                using (MemoryStream ms = streamManager.GetStream())
                {
                    IMinecraftPrimitiveWriter writer = new MinecraftPrimitiveWriter(ms);
                    packet.Write(writer);
                    ms.Position = 0;
                    await minecraftProtocol.SendPacketAsync(new (id, ms), cancellationToken);
                }
            }
            else
            {
                throw new InvalidOperationException($"Output Packet {id} notFound");
            }

        }
        public void SendPacket(MinecraftPacket packet)
        {
            bool ok = packets.TryGetOutputId(packet, out int id);
            if (ok)
            {
                using (MemoryStream data = new MemoryStream())
                {
                    IMinecraftPrimitiveWriter writer = new MinecraftPrimitiveWriter(data);
                    packet.Write(writer);
                    data.Position = 0;
                    minecraftProtocol.SendPacket(new(id, data));

                }
            }
            else
            {
                throw new InvalidOperationException($"Output Packet {id} notFound");
            }
        }
        ~PacketReaderWriter()
        {
            Dispose();
        }
        private bool disposed = false;

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            if (_disposeProtocol)
            {
                minecraftProtocol?.Dispose();
                minecraftProtocol = null;
            }
            packets?.Dispose();
            packets = null;
            GC.SuppressFinalize(this);
        }

        public void SwitchEncryption(byte[] privateKey)
        {
            minecraftStream.SwitchEncryption(privateKey);
        }

        public void SwitchCompression(int threshold)
        {
            minecraftProtocol.SwitchCompression(threshold);
        }


    }
}

