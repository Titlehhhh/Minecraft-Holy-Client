﻿using DynamicData;

namespace McProtoNet.Protocol
{
	public static class Extensions
	{
		public static Position ReadPosition(this IMinecraftPrimitiveReader reader)
		{
			long locEncoded = reader.ReadLong();


			int x, y, z;
			//if (protocolversion >= Protocol18Handler.MC_1_14_Version)
			//{
			x = (int)(locEncoded >> 38);
			y = (int)(locEncoded & 4095);
			z = (int)(locEncoded << 26 >> 38);
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
	}
}