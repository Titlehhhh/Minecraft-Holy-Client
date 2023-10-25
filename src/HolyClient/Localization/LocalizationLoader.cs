using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HolyClient.Localization
{
	/// <summary>
	/// Cette classe permet de charger les traduction en mémoire (soit par code, par fichier ou par dossier)
	/// </summary>
	public class LocalizationLoader
	{
		private readonly Loc localization;

		public LocalizationLoader(Loc localization)
		{
			this.localization = localization;
		}

		private static LocalizationLoader instance;

		public static LocalizationLoader Instance
		{
			get
			{
				return instance ?? (instance = new LocalizationLoader(Loc.Instance));
			}
		}

		public List<ILocalizationFileLoader> FileLanguageLoaders { get; set; } = new List<ILocalizationFileLoader>();

		/// <summary>
		/// Add a new translation in the languages dictionaries
		/// </summary>
		/// <param name="textId">The text to translate identifier</param>
		/// <param name="languageId">The language identifier of the translation</param>
		/// <param name="value">The value of the translated text</param>
		public void AddTranslation(string textId, string languageId, string value, string source = "")
		{
			if (!localization.TranslationsDictionary.ContainsKey(textId))
				localization.TranslationsDictionary[textId] = new SortedDictionary<string, LocTranslation>();

			if (!localization.AvailableLanguages.Contains(languageId))
				localization.AvailableLanguages.Add(languageId);

			localization.TranslationsDictionary[textId][languageId] = new LocTranslation()
			{
				TextId = textId,
				LanguageId = languageId,
				TranslatedText = value,
				Source = source
			};
		}

		/// <summary>
		/// Load the specified file in the Languages dictionnaries
		/// </summary>
		/// <param name="fileName">The filename of the file to load</param>
		public void AddFile(string fileName)
		{
			FileLanguageLoaders.Find(loader => loader.CanLoadFile(fileName))?.LoadFile(fileName, this);
		}

		public void AddFile(Stream file)
		{
			foreach (var item in FileLanguageLoaders)
			{
				item.LoadFile(file, this);
			}
		}



		/// <summary>
		/// Load all the language files of the specified directory in the languages dictionnaries
		/// </summary>
		/// <param name="path">The path of the directory to load</param>
		/// <param name="recursive">Specify if files are loaded in subdirectories or not</param>
		public void AddDirectory(string path, bool recursive = false)
		{
			Directory.GetFiles(path)
				.ToList()
				.ForEach(AddFile);

			if (recursive)
			{
				Directory.GetDirectories(path)
					.ToList()
					.ForEach(directory => AddDirectory(directory, recursive));
			}
		}

		/// <summary>
		/// Remove all translation that comes from the specified source
		/// </summary>
		/// <param name="source">The fileName or source of the translation</param>
		public void RemoveAllFromSource(string source)
		{
			localization.TranslationsDictionary.Keys.ToList().ForEach(textId =>
			{
				localization.TranslationsDictionary[textId].Values.ToList().ForEach(translation =>
				{
					if (translation.Source.Equals(source))
					{
						localization.TranslationsDictionary[textId].Remove(translation.LanguageId);
					}
				});

				if (localization.TranslationsDictionary[textId].Count == 0)
				{
					localization.TranslationsDictionary.Remove(textId);
				}
			});
		}

		/// <summary>
		/// Empty All Dictionnaries
		/// </summary>
		public void ClearAllTranslations(bool clearAlsoAvailableLanguages = false)
		{
			localization.TranslationsDictionary.Clear();
			if (clearAlsoAvailableLanguages)
				localization.AvailableLanguages.Clear();
			localization.MissingTranslations.Clear();
		}
	}
}
