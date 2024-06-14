using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HolyClient.Converters;

public class MinecraftVersionToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MinecraftVersion version) return McVerToString(version);
        throw new InvalidOperationException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static string McVerToString(MinecraftVersion version)
    {
        string v = version.ToString();
        v = v.ToLower().Replace("mc", "").Replace("version", "");
        v = v.Trim('_').Replace("_", ".");
        return v;
    }
}