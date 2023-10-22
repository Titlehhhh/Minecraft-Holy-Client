using System;
using System.Collections.Generic;

namespace HolyClient.Localization
{
	public class LocalizationMissingTranslationEventArgs : EventArgs
	{
		public LocalizationMissingTranslationEventArgs(Loc loc, SortedDictionary<string, SortedDictionary<string, string>> missingTranslations, string textId)
		{
			Loc = loc;
			MissingTranslations = missingTranslations;
			TextId = textId;
		}

		public Loc Loc { get; }

		public SortedDictionary<string, SortedDictionary<string, string>> MissingTranslations { get; }

		public string TextId { get; }

		public string LanguageId { get; }
	}
}
