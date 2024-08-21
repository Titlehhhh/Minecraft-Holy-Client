using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace HolyClient.Converters;

public class DownloadCountFormatConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long count)
        {
            if (count < 1000)
                return count;

            var suffix = " тыс.";
            long index = 3;
            if (count >= 1_000_000_000L)
            {
                suffix = " млрд.";
                index = 9;
            }
            else if (count >= 1_000_000L)
            {
                suffix = " млн.";
                index = 6;
            }


            if (string.IsNullOrEmpty(suffix)) return count.ToString();
            var result = ToString(count, index);

            result = FormatString(result);

            return result + suffix;
        }

        return "-1";
    }


    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string FormatString(string a)
    {
        return a.Substring(0, 4)
            .TrimEnd('0')
            .TrimEnd(',');
    }

    private static string ToString(long value, long index)
    {
        if (value == 0)
            return "0";
        var result = "";
        long i = 0;
        while (value > 0)
        {
            var z = value % 10;
            value /= 10;
            if (index == i) result = "," + result;
            result = z + result;
            i++;
        }

        return result;
    }
}