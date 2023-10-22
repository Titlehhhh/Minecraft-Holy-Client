namespace HolyClient.Localization
{
	/// <summary>
	/// Formatting interface
	/// </summary>
	public interface IFormatter
	{
		/// <summary>
		/// Format a string with the format and the associated model
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="model">An object to inject as the model</param>
		/// <returns>The formated string if the formatter know how to format the string with the model, otherwise should return unchanged <see cref="format"/></returns>
		string Format(string format, object model);
	}
}
