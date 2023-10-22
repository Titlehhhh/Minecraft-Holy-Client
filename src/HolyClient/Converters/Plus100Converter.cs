using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HolyClient.Converters
{
	public class Plus100Converter : IValueConverter
	{
		public static Plus100Converter Instance { get; } = new();
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is double v)
			{
				return v + 100;
			}
			return value;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
