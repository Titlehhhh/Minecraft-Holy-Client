class Program
{
    private volatile int _dispoed = 0;

    static void Main(string[] args)
    {
        NewMethod();
        
    }

    
    
    private static void NewMethod()
    {
        List<KeyValuePair<int, int>> list = new();
        for (int i = 340; i <= 767; i++)
        {
            var g = GetClientboundConfigPacket(i, "DisconnectConfigurationPacket");
            list.Add(new KeyValuePair<int, int>(i, g));
        }

        var ranges = list.GroupAdjacent(c => c.Value)
            .Select(g => new VersionRange(
                g.Min(c => c.Key),
                g.Max(c => c.Key),
                g.Key));


        foreach (var r in ranges)
        {
            Console.WriteLine(r.ToSwitchCaseSend() + ",");
        }


        return;
    }

    private static int GetServerboundPlayPacket(int version, string packet)
    {
        return Ext.ServerboundPlayPackets(version).IndexOf("Serverbound" + packet);
    }

    private static int GetClientboundPlayPacket(int version, string packet)
    {
        return Ext.ClientboundPlayPackets(version).IndexOf("Clientbound" + packet);
    }
    
    private static int GetClientboundConfigPacket(int version, string packet)
    {
        return Ext.ClientboundConfigurationPackets(version).IndexOf("Clientbound" + packet);
    }
    private static int GetServerboundConfigPacket(int version, string packet)
    {
        return Ext.ServerboundConfigurationPackets(version).IndexOf("Serverbound" + packet);
    }
    
    private static int GetClientboundLoginPacket(int version, string packet)
    {
        return Ext.ClientboundLoginPackets(version).IndexOf("Clientbound" + packet);
    }
    private static int GetServerboundLoginPacket(int version, string packet)
    {
        return Ext.ServerboundLoginPackets(version).IndexOf("Serverbound" + packet);
    }
}