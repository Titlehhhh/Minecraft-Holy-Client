using McProtoNet.Geometry;

namespace McProtoNet.MultiVersion.Events
{
    public class SpawnPlayerEventArgs : EventArgs
    {
        public int EntityId { get; private set; }
        public Guid UUID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public byte Yaw { get; private set; }
        public byte Pitch { get; private set; }

        public SpawnPlayerEventArgs(int entityId, Guid uUID, double x, double y, double z, byte yaw, byte pitch)
        {
            EntityId = entityId;
            UUID = uUID;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public Vector3 GetPosition() => new Vector3(X, Y, Z);

    }
}
