namespace HolyClient.StressTest;

internal class NickProvider : INickProvider
{
    private readonly string _baseNick;
    private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public NickProvider(string baseNick)
    {
        if (string.IsNullOrWhiteSpace(baseNick))
            baseNick = "";
        if (baseNick.Length > 16)
            throw new ArgumentException("Nick long");
        _baseNick = baseNick;
    }

    public string GetNextNick()
    {
        var stringChars = new char[10 - _baseNick.Length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

        var finalString = new string(stringChars);
        return finalString + _baseNick;
    }
}