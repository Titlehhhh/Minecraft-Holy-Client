using System.Text.RegularExpressions;

namespace HolyClient.Localization;

/// <summary>
///     Formatter to select the string with a boolean value
/// </summary>
public class TernaryFormatter : IFormatter
{
	/// <summary>
	///     Format a string from <see cref="model" /> as a Boolean value
	/// </summary>
	/// <param name="format">Format as true value | false value</param>
	/// <param name="model">The <see cref="model" /> that should be a bool</param>
	/// <returns>
	///     The formated string if the formatter know how to format the string with the model, otherwise should return
	///     unchanged <see cref="format" />
	/// </returns>
	public string Format(string format, object model)
    {
        if (!(model is bool))
            return format;

        var conditionFormat = Regex.Split(format, @"\s\|\s");
        if (conditionFormat.Length > 1)
            return conditionFormat[(bool)model ? 0 : 1];
        return format;
    }
}