using System.IO;

namespace HolyClient.Localization
{
	/// <summary>
	/// Interface to implement for adding a file format to the localization system
	/// To Add to LocLanguagesLoader.FileLanguageLoaders List
	/// </summary>
	public interface ILocalizationFileLoader
	{
		/// <summary>
		/// Must return true if the current LanguageFileLoader is able to load (decode) the format of the specified file.
		/// Must return false otherwise.
		/// </summary>
		/// <param name="fileName">The file we want to load</param>
		/// <returns></returns>
		bool CanLoadFile(string fileName);

		/// <summary>
		/// Must decode the specified file and can use loader.AddTranslation(textId, languageId, value, source) to load each translation
		/// </summary>
		/// <param name="fileName">The file we want to load</param>
		/// <param name="loader">The loader to use to load a single translation</param>
		void LoadFile(string fileName, LocalizationLoader loader);


		void LoadFile(Stream file, LocalizationLoader loader);

	}
}
