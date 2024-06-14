using System;

namespace HolyClient.ViewModels;

public sealed class ExceptionInfoViewModel
{
    public ExceptionInfoViewModel(string type, string message, int count)
    {
        Type = type;
        Message = message;
        Count = count;
        Key = Tuple.Create(Type, Message);
    }

    public string Type { get; }
    public string Message { get; }
    public int Count { get; private set; }

    public Tuple<string, string> Key { get; private set; }
}