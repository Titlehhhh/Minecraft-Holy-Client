using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.Protocol756
{
    public static class Extensions
    {
        private const int POSITION_X_SIZE = 38;
        private const int POSITION_Y_SIZE = 12;
        private const int POSITION_Z_SIZE = 38;
        private const int POSITION_Y_SHIFT = 0xFFF;
        private const int POSITION_WRITE_SHIFT = 0x3FFFFFF;
        public static Vector3 ReadPosition(this IMinecraftPrimitiveReader reader)
        {
            long val = reader.ReadLong();
            int x = (int)(val >> POSITION_X_SIZE);
            int y = (int)(val & POSITION_Y_SHIFT);
            int z = (int)(val << 26 >> POSITION_Z_SIZE);
            return new Vector3(x, y, z);
        }
        public static void WritePosition(this IMinecraftPrimitiveWriter writer, Vector3 point)
        {
            long x = (int)point.X & POSITION_WRITE_SHIFT;
            long y = (int)point.Y & POSITION_Y_SHIFT;
            long z = (int)point.Z & POSITION_WRITE_SHIFT;

            writer.WriteLong(x << POSITION_X_SIZE | z << POSITION_Y_SIZE | y);
        }
    }
}
