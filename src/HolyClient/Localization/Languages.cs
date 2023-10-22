using System.Reflection;

namespace HolyClient.Localization
{
	public static class Languages
	{


		public static void Init()
		{
			Loc.Instance.LogOutMissingTranslations = true;

			LocalizationLoader.Instance.FileLanguageLoaders.Add(new JsonFileLoader());

			ReloadFiles();
		}

		public static void ReloadFiles()
		{

			LocalizationLoader.Instance.ClearAllTranslations();

			using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HolyClient.lang.words.loc.json");

			LocalizationLoader.Instance.AddFile(stream);
		}
	}
}
