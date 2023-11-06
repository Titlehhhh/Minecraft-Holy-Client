namespace HolyClient.Localization
{
	/// <summary>
	/// This interface describe a translator that provide a translation for a textId and a languageId
	/// </summary>
	public interface ITranslator
	{
		/// <summary>
		/// To ask if this translator can translate the given textId and languageId
		/// </summary>
		/// <param name="textId">the text id of the translation</param>
		/// <param name="languageId">the language Id of the translation</param>
		/// <returns><c>true</c> if it can translate.Otherwise <c>false</c></returns>
		bool CanTranslate(string textId, string languageId);

		/// <summary>
		/// Translate the given textId and languageId
		/// </summary>
		/// <param name="textId">the text id to translate</param>
		/// <param name="languageId">the languageId in which to translate</param>
		/// <returns>The text of the translated textId in the languageId, or null if it can't translate.</returns>
		string Translate(string textId, string languageId);
	}
}
