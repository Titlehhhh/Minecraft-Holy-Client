using McProtoNet.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.MultiVersion.Events
{

    public class PlayerPositionRotationEventArgs : EventArgs
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public float Yaw { get; }
        public float Pitch { get; }

        public byte Flags { get; }
        public int TeleportId { get; }

        public PlayerPositionRotationEventArgs(double x, double y, double z, float yaw, float pitch, byte flags, int teleportId)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;

            Flags = flags;
            TeleportId = teleportId;
        }

        public Vector3 GetPosition(Vector3 current)
        {
            byte locMask = Flags;

            current.X = (locMask & 1 << 0) != 0 ? current.X + X : X;
            current.Y = (locMask & 1 << 1) != 0 ? current.Y + Y : Y;
            current.Z = (locMask & 1 << 2) != 0 ? current.Z + Z : Z;
            return current;
        }
        public Vector3 GetPosition()
        {

            return new Vector3(X, Y, Z);
        }
        public Rotation GetRotation()
        {
            return new Rotation(Yaw, Pitch);
        }
    }

}
