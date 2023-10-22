namespace McProtoNet.Geometry
{
    public struct Rotation
    {
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public Vector3 Vector { get; private set; }
        private const double RadToDeg = 57.29577951308232067679;
        public Rotation(Vector3 vector)
        {
            Vector = vector;

            double r = vector.Distance;
            Yaw = (float)(-Math.Atan2(vector.X, vector.Z) * RadToDeg);
            if (Yaw < 0)
                Yaw += 360;
            Pitch = (float)(-Math.Asin(vector.Y / r) * RadToDeg);
        }
        public Rotation(float yaw, float pitch)
        {
            this.Yaw = yaw;
            this.Pitch = pitch;
            Vector = new Vector3(-Math.Cos(Pitch) * Math.Sin(Yaw), -Math.Sin(pitch), Math.Cos(Pitch) * Math.Cos(Yaw));

        }
    }
}
