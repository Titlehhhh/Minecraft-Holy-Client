namespace SourceGenerator
{
    class ProxyContainer
    {
        List<(string, ushort)> proxies;
        public ProxyContainer()
        {
            proxies = new List<(string, ushort)>();
           
            Console.WriteLine("Load Proxy");
            using (StreamReader sr = new StreamReader("S4Proxies.txt"))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        var line = sr.ReadLine().Trim();
                        string[] HostPort = line.Split(':');
                        proxies.Add((HostPort[0], ushort.Parse(HostPort[1])));
                    }
                    catch
                    {

                    }
                }

            }
            Console.WriteLine("Proxies: " + proxies.Count);
        }
        private volatile int index = 0;
        private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        public async ValueTask<(string, ushort)> GetProxyAsync()
        {
            try
            {
                await _lock.WaitAsync();

                if (index == proxies.Count)
                    index = 0;
                return proxies[index++];
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
