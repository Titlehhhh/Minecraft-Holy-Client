public class MinecraftDataContext
{
    private int _maxVersion;
    private int _minVersion;

    public MinecraftDataContext(int minVersion, int maxVersion, string dir)
    {
        _minVersion = minVersion;
        _maxVersion = maxVersion;
    }

    public void GenerateNames()
    {
    }
}