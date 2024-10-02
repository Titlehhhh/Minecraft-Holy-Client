class Program
{
    private volatile int _dispoed = 0;

    static void Main(string[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            new Program().Go();
        }
    }

    private void Go()
    {
        var actions = Enumerable.Repeat<Action>(InvokeGo, 100).ToArray();
        Parallel.Invoke(actions);
    }

    private void InvokeGo()
    {
        Thread.Sleep(10);
        
        
        
        if (Interlocked.CompareExchange(ref _dispoed, 1, 0) == 0)
        {
            Console.WriteLine("AAAAAAAAA");
        }
    }
}