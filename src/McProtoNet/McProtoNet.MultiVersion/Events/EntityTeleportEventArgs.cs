using McProtoNet.Geometry;

namespace McProtoNet.MultiVersion.Events
{
    public class EntityTeleportEventArgs : EventArgs
    {
        public int EntityId { get; private set; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public byte Yaw { get; private set; }
        public byte Pitch { get; private set; }

        public bool OnGround { get; private set; }

        public EntityTeleportEventArgs(int entityId, double x, double y, double z, byte yaw, byte pitch, bool onGround)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public Vector3 GetVector()
        {
            return new Vector3(X, Y, Z);
        }

    }

}
