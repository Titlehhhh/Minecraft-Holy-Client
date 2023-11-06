using System;
using System.Text.RegularExpressions;

namespace HolyClient.Localization
{
	/// <summary>
	/// Format to pluralize a string with a format and a model
	/// </summary>
	public class PluralizationFormatter : IFormatter
	{
		/// <summary>
		/// Pluralize the string
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="model">The model that should be a number</param>
		/// <returns>The formated string if the formatter know how to format the string with the model, otherwise should return unchanged <see cref="format"/></returns>
		public string Format(string format, object model)
		{
			if (!(model is SByte || model is Int16 || model is Int32 || model is Int64 ||
				model is Byte || model is UInt16 || model is UInt32 || model is UInt64))
				return format;

			string[] stringFormatMapper = new string[3];
			var pluralizeFormat = Regex.Split(format, @"\s\|\s");
			if (pluralizeFormat.Length > 2)
			{
				stringFormatMapper[0] = pluralizeFormat[0];
				stringFormatMapper[1] = pluralizeFormat[1];
				stringFormatMapper[2] = pluralizeFormat[2];
			}
			else if (pluralizeFormat.Length > 1)
			{
				stringFormatMapper[0] = pluralizeFormat[0];
				stringFormatMapper[1] = pluralizeFormat[0];
				stringFormatMapper[2] = pluralizeFormat[1];
			}
			else
			{
				stringFormatMapper[0] = pluralizeFormat[0];
				stringFormatMapper[1] = pluralizeFormat[0];
				stringFormatMapper[2] = pluralizeFormat[0];
			}

			var formatIndex = Convert.ToInt64(model);
			formatIndex = formatIndex > 2 ? 2 : formatIndex;
			return stringFormatMapper[formatIndex];
		}
	}
}
