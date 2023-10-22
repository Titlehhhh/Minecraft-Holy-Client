using McProtoNet.Geometry;

namespace McProtoNet.MultiVersion.Events
{
    public class EntityPositionRotationEventArgs : EventArgs
    {
        public int EntityId { get; private set; }
        public double DeltaX { get; }
        public double DeltaY { get; }
        public double DeltaZ { get; }

        public byte Yaw { get; }
        public byte Pitch { get; }

        public bool OnGround { get; private set; }

        public EntityPositionRotationEventArgs(int entityId, double deltaX, double deltaY, double deltaZ, byte yaw, byte pitch, bool onGround)
        {
            EntityId = entityId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public Vector3 GetVector()
        {
            return new Vector3(DeltaX, DeltaY, DeltaZ);
        }

    }

}
