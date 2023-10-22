namespace McProtoNet.Utils
{
    public class Rcon :  IDisposable
    {
        private readonly string host;
        private readonly ushort port;
        private readonly string password;
        public Rcon(string host, ushort port = 25575, string password = "")
        {
            this.host = host;
            this.port = port;
            this.password = password;

        }

        public void SendMessage()
        {

        }

        public void Dispose()
        {

        }
    }
}
