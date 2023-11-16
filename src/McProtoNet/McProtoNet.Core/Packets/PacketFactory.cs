namespace McProtoNet.Core.Packets
{


	public class PacketFactory : IPacketFactory
	{

		protected virtual (Dictionary<int, Type>, Dictionary<int, Type>) CreatePackets(PacketCategory category)
		{
			return (null, null);
		}

		public virtual IPacketProvider CreateProvider(PacketCategory packetCategory, PacketSide side)
		{
			Dictionary<int, Type> outPackets = null;
			Dictionary<int, Type> inPackets = null;
			if (packetCategory == PacketCategory.HandShake)
			{
				outPackets = new Dictionary<int, Type>
				{
					{ 0x00, typeof(HandShakePacket) }
				};
				inPackets = new();
			}
			else if (packetCategory == PacketCategory.Status)
			{
				outPackets = new Dictionary<int, Type>
				{
					{ 0x00, typeof(StatusQueryPacket) },
					{ 0x01, typeof(StatusPingPacket) },
				};
				inPackets = new Dictionary<int, Type>
				{
					{ 0x00, typeof(StatusResponsePacket) },
					{ 0x01, typeof(StatusPongPacket) },
				};
			}
			else
			{
				(outPackets, inPackets) = CreatePackets(packetCategory);
			}
			if (inPackets is null || outPackets is null)
				throw new InvalidOperationException("Словари с пакетами не заполнены");
			if (side == PacketSide.Server)
				(inPackets, outPackets) = (outPackets, inPackets);
			return new PacketProvider(outPackets, inPackets);
		}
	}
}
