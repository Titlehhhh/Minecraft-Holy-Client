using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Data.Converters;
using HolyClient.Common;

namespace HolyClient.Converters;

public class DataGridProxyItemsSourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return Enumerable.Empty<ProxyInfo>();
        if (value is IEnumerable<ProxyInfo> proxies)
            return new DataGridCollectionView(proxies)
            {
                GroupDescriptions =
                {
                    new DataGridPathGroupDescription(nameof(ProxyInfo.Type))
                }
            };
        throw new Exception("Only Proxy");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}