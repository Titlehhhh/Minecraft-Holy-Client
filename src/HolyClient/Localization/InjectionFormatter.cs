using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HolyClient.Localization;

/// <summary>
///     Formatter to inject the model property inside the object
/// </summary>
public class InjectionFormatter : IFormatter
{
    private readonly Regex propertyMustachesRegex =
        new(@"\{(?<object>[A-Za-z]+[A-Za-z0-9]*)(\.(?<property>[A-Za-z0-9]*)){0,}\}");

    /// <summary>
    ///     Format a string by injecting model property.
    /// </summary>
    /// <param name="format">Format string</param>
    /// <param name="model">The model that provides the property</param>
    /// <returns>
    ///     The formated string if the formatter know how to format the string with the model, otherwise should return
    ///     unchanged <see cref="format" />
    /// </returns>
    public string Format(string format, object model)
    {
        if (model == null)
            return format;

        var matches = propertyMustachesRegex.Matches(format);
        foreach (Match match in matches)
        {
            object value = null;

            if (model is Dictionary<string, object> dictionary)
            {
                string dictionnaryObject = null;
                foreach (Capture capture in match.Groups["object"].Captures) dictionnaryObject = capture.Value;

                value = dictionary[dictionnaryObject];
            }
            else
            {
                value = model;
            }

            foreach (Capture capture in match.Groups["property"].Captures)
            {
                var property = value.GetType().GetProperty(capture.Value);
                if (property != null) value = property.GetValue(value);
                else
                    value = null;
                if (value == null) break;
            }

            format = format.Replace(match.Value, value?.ToString() ?? match.Value);
        }

        return format;
    }
}