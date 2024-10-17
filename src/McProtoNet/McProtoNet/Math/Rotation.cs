namespace McProtoNet;

public struct Rotation
{
    public float Yaw { get; }
    public float Pitch { get; }

    public Vector3 Vector { get; private set; }

    public Rotation(Vector3 vector)
    {
        Vector = vector;

        var r = vector.Distance;
        Yaw = (float)double.RadiansToDegrees(-Math.Atan2(vector.X, vector.Z));
        if (Yaw < 0)
            Yaw += 360;
        Pitch = (float)double.RadiansToDegrees(-Math.Asin(vector.Y / r));
    }

    public Rotation(float yaw, float pitch)
    {
        Yaw = yaw;
        Pitch = pitch;
        Vector = new Vector3(-Math.Cos(Pitch) * Math.Sin(Yaw), -Math.Sin(pitch), Math.Cos(Pitch) * Math.Cos(Yaw));
    }

    public override string ToString()
    {
        return $"Yaw: {Yaw}, Pitch: {Pitch}";
    }
}