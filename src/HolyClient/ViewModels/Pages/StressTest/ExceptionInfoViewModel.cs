using System;


namespace HolyClient.ViewModels;

public sealed class ExceptionInfoViewModel
{
	public string Type { get; private set; }
	public string Message { get; private set; }
	public int Count { get; private set; }

	public Tuple<string, string> Key { get; private set; }

	public ExceptionInfoViewModel(string type, string message, int count)
	{
		Type = type;
		Message = message;
		Count = count;
		Key = Tuple.Create<string, string>(Type, Message);
	}
}
