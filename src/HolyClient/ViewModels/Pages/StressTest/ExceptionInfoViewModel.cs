using System;
using System.Threading;

namespace HolyClient.ViewModels;

public sealed class ExceptionInfoViewModel
{
    private volatile int _count;

    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }
    public ExceptionInfoViewModel(Tuple<string,string> key, int count)
    {
        Type = key.Item1;
        Message = key.Item2;
        Count = count;
        Key = key;
    }

    public string Type { get; }
    public string Message { get; }

    public int Count
    {
        get => _count;
        private set => _count = value;
    }

    public Tuple<string, string> Key { get; private set; }
}