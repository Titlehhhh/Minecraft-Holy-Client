using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace HolyClient.Localization
{
	/// <summary>
	/// The base class of the localization system
	/// To Translate a text use Loc.Tr()
	/// </summary>
	public class Loc : ReactiveObject
	{
		private static readonly object lockObject = new object();

		private string currentLanguage = "en";

		/// <summary>
		/// The current language used for displaying texts with Localization
		/// By default if not set manually is equals to "en" (English)
		/// </summary>

		public string CurrentLanguage
		{
			get { return currentLanguage; }
			set
			{
				if (value != null)
				{
					if (!AvailableLanguages.Contains(value))
					{
						AvailableLanguages.Add(value);
					}

					if (!currentLanguage.Equals(value))
					{
						CurrentLanguageChangingEventArgs changingArgs = new CurrentLanguageChangingEventArgs(currentLanguage, value);
						CurrentLanguageChangedEventArgs changedArgs = new CurrentLanguageChangedEventArgs(currentLanguage, value);

						CurrentLanguageChanging?.Invoke(this, changingArgs);

						if (!changingArgs.Cancel)
						{
							currentLanguage = value;
							CurrentLanguageChanged?.Invoke(this, changedArgs);
							this.RaisePropertyChanged(nameof(CurrentLanguage));
						}
					}
				}
			}
		}

		/// <summary>
		/// The language to use if a translation is missing in the <see cref="CurrentLanguage"/>.<para/>
		/// If the translation is also missing in the <see cref="FallbackLanguage"/> the defaultText if specified is then used.<para/>
		/// To use it set <see cref="UseFallbackLanguage"/> to <c>true</c><para/>
		/// By default if not set manually is equals to "en" (English)
		/// </summary>
		[Reactive]
		public string FallbackLanguage { get; set; } = "en";

		/// <summary>
		/// if set to <c>true</c> use <see cref="FallbackLanguage"/> when a translation is missing.<para/>
		/// if set to <c>false</c> <see cref="FallbackLanguage"/> is not used and the defaultText if specified is directly used.<para/>
		/// Default value is  : <c>false</c>
		/// </summary>
		[Reactive]
		public bool UseFallbackLanguage { get; set; }

		/// <summary>
		/// Fired when the current language is about to change
		/// Can be canceled
		/// </summary>
		public event EventHandler<CurrentLanguageChangingEventArgs> CurrentLanguageChanging;

		/// <summary>
		/// Fired when the current language has changed.
		/// </summary>
		public event EventHandler<CurrentLanguageChangedEventArgs> CurrentLanguageChanged;

		/// <summary>
		/// The List of all availables languages where at least one translation is present.
		/// </summary>
		public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>();

		/// <summary>
		/// The list of translators that try to translate each text.
		/// first translators have higher priorities than last
		/// </summary>
		public List<ITranslator> Translators { get; } = new List<ITranslator>();

		public List<IFormatter> Formatters { get; set; } = new List<IFormatter>
		{
			new PluralizationFormatter(),
			new TernaryFormatter(),
			new InjectionFormatter(),
		};

		/// <summary>
		/// The dictionary that contains all available translations
		/// (TranslationsDictionary[TextId][LanguageId])
		/// </summary>
		public SortedDictionary<string, SortedDictionary<string, LocTranslation>> TranslationsDictionary { get; set; } = new SortedDictionary<string, SortedDictionary<string, LocTranslation>>();

		private static Loc instance;

		/// <summary>
		/// The default instance
		/// </summary>
		public static Loc Instance
		{
			get
			{
				if (instance == null)
				{
					lock (lockObject)
					{
						if (instance == null)
						{
							instance = new Loc();
						}
					}
				}

				return instance;
			}
		}

		/// <summary>
		/// Default Constructor
		/// Use a <see cref="FilesDictionaryTranslator"/>
		/// </summary>
		public Loc()
		{



			Translators.Add(
				new FilesDictionaryTranslator()
				{
					Loc = this
				});
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="translators">a collection of translators to use to translate texts</param>
		public Loc(params ITranslator[] translators)
		{
			Translators.AddRange(translators);
		}


		/// <summary>
		/// To fire the event <see cref="PropertyChanged"/> for the specified property.<para/>
		/// If no <see cref="propertyName"/> specified use the name of the calling property
		/// </summary>
		/// <param name="propertyName">The name of the property that have changed</param>


		/// <summary>
		/// To Manually raise CurrentLanguageChanging and CurrentLanguageChanged events.
		/// Force events even if the current language didn't changed.
		/// </summary>
		public virtual void RaiseLanguageChangeEvents()
		{
			var changingArgs = new CurrentLanguageChangingEventArgs(currentLanguage, currentLanguage);
			var changedArgs = new CurrentLanguageChangedEventArgs(currentLanguage, currentLanguage);

			CurrentLanguageChanging?.Invoke(this, changingArgs);

			if (!changingArgs.Cancel)
			{
				CurrentLanguageChanged?.Invoke(this, changedArgs);
				this.RaisePropertyChanged(nameof(CurrentLanguage));
			}
		}

		/// <summary>
		/// Translate the given textId in current language.
		/// This method is a shortcut to Instance.Translate
		/// </summary>
		/// <param name="textId">The text to translate identifier</param>
		/// <param name="defaultText">The text to return if no text correspond to textId in the current language</param>
		/// <param name="languageId">The language id in which to get the translation. To Specify it if not CurrentLanguage</param>
		/// <returns>The translated text</returns>
		public static string Tr(string textId, string defaultText = null, string languageId = null)
		{
			return Instance.Translate(textId, defaultText, languageId);
		}

		/// <summary>
		/// Translate the given textId in current language and format it with <see cref="model"/> data.
		/// This method is a shortcut to Instance.Translate
		/// </summary>
		/// <param name="textId">The text to translate identifier</param>
		/// <param name="model">Data to use for formatting</param>
		/// <param name="defaultText">The text to return if no text correspond to textId in the current language</param>
		/// <param name="languageId">The language id in which to get the translation. To Specify it if not CurrentLanguage</param>
		/// <returns>The translated text</returns>
		public static string Tr(string textId, object model, string defaultText = null, string languageId = null)
		{
			return Instance.Translate(textId, model, defaultText, languageId);
		}

		/// <summary>
		/// Translate the given textId in current language.
		/// </summary>
		/// <param name="textId">The text to translate identifier</param>
		/// <param name="defaultText">The text to return if no text correspond to textId in the current language</param>
		/// <param name="languageId">The language id in which to get the translation. To Specify if not <see cref="CurrentLanguage"/></param>
		/// <returns>The translated text</returns>
		public virtual string Translate(string textId, string defaultText = null, string languageId = null)
		{
			if (string.IsNullOrEmpty(defaultText))
			{
				defaultText = textId;
			}

			if (string.IsNullOrEmpty(languageId))
			{
				languageId = CurrentLanguage;
			}

			CheckMissingTranslation(textId ?? string.Empty, defaultText);

			return Translators
				.Find(tr => tr.CanTranslate(textId, languageId))?
				.Translate(textId ?? string.Empty, languageId) ??

				(UseFallbackLanguage ? Translators
				.Find(tr => tr.CanTranslate(textId, FallbackLanguage))?
				.Translate(textId ?? string.Empty, FallbackLanguage) : null)

				?? defaultText ?? string.Empty;
		}

		/// <summary>
		/// Translate the given textId in current language and format it with <see cref="model"/> data.
		/// </summary>
		/// <param name="textId">The text to translate identifier</param>
		/// <param name="model">A Model to inject in translated text</param>
		/// <param name="defaultText">The text to return if no text correspond to textId in the current language</param>
		/// <param name="languageId">The language id in which to get the translation. To Specify if not CurrentLanguage</param>
		/// <returns>The translated text</returns>
		public string Translate(
			string textId,
			object model,
			string defaultText = null,
			string languageId = null)
		{
			var format = Translate(textId, defaultText, languageId);

			Formatters.ForEach(formatter => format = formatter.Format(format, model));

			return format;
		}

		/// <summary>
		/// For developpers, for developement and/or debug time.
		/// If set to <c>True</c> Log Out textId asked to be translate but missing in the specified languageId.
		/// Fill the MissingTranslations Dictionary
		/// </summary>
		public bool LogOutMissingTranslations { get; set; }

		public SortedDictionary<string, SortedDictionary<string, string>> MissingTranslations { get; } = new SortedDictionary<string, SortedDictionary<string, string>>();

		protected virtual void CheckMissingTranslation(string textId, string defaultText)
		{
			if (LogOutMissingTranslations)
			{
				bool needLogUpdate = false;

				AvailableLanguages.Where(al => al != null).ToList().ForEach(languageId =>
				{
					if (!Translators.Any(tr => tr.CanTranslate(textId, languageId)))
					{
						if (!MissingTranslations.ContainsKey(textId))
						{
							MissingTranslations.Add(textId, new SortedDictionary<string, string>());
						}

						MissingTranslations[textId][languageId] = $"default text : {defaultText}";

						needLogUpdate = true;
					}
				});

				if (needLogUpdate)
					MissingTranslationFound?.Invoke(this, new LocalizationMissingTranslationEventArgs(this, MissingTranslations, textId));
			}
		}

		/// <summary>
		/// For developpers, for developement and/or debug time.
		/// Fired to inform some translation are missing.
		/// LogOutMissingTranslations must be set to true
		/// </summary>
		public event EventHandler<LocalizationMissingTranslationEventArgs> MissingTranslationFound;
	}
}
