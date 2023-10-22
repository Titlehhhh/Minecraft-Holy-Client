using McProtoNet.Geometry;

namespace McProtoNet.MultiVersion.Events
{
    public class EntityPositionEventArgs : EventArgs
    {
        public int EntityId { get; private set; }
        public double DeltaX { get; }
        public double DeltaY { get; }
        public double DeltaZ { get; }
        public bool OnGround { get; private set; }

        public EntityPositionEventArgs(int entityId, double deltaX, double deltaY, double deltaZ, bool onGround)
        {
            EntityId = entityId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            OnGround = onGround;
        }

        public Vector3 GetVector()
        {
            return new Vector3(DeltaX, DeltaY, DeltaZ);
        }

    }

}
