class VersionRange
{
    public int Min { get; }
    public int Max { get; }
    public int Id { get; }

    public VersionRange(int min, int max, int id)
    {
        Min = min;
        Max = max;
        Id = id;
    }

    public string ToSwitchCaseSend()
    {
        if (Min != Max)
        {
            return $">= {Min} and <= {Max} => {IdToStr()}";
        }

        return $"{Min} => {IdToStr()}";
    }

    private string IdToStr()
    {
        if (Id < 0)
            return "-1";

        return $"0x{Id:X2}";
    }

    public override string ToString()
    {
        return $"From {Min} to {Max} id {Id}";
    }
}