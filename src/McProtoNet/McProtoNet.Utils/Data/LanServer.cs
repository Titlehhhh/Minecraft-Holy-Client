namespace McProtoNet.Utils
{
    public struct LanServer
    {
        public string Host { get; private set; }
        public ushort Port { get; private set; }
        public string Motd { get; private set; }

        public LanServer(string host, ushort port, string motd)
        {
            Host = host;
            Port = port;

            Motd = motd;
        }
    }
}