using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HolyClient.Localization
{
	/// <summary>
	/// Converter to Translate a specific TextId in the Binding LanguageId.
	/// If Translation don't exist return DefaultText.
	/// Not usable in TwoWay Binding mode.
	/// </summary>
	public class TrLanguageIdConverter : IValueConverter
	{
		/// <summary>
		/// To force the use of a specific identifier
		/// </summary>
		public virtual string TextId { get; set; }

		/// <summary>
		/// The text to return if no text correspond to textId in the current language
		/// </summary>
		public string DefaultText { get; set; }

		/// <summary>
		/// To provide a prefix to add at the begining of the translated text.
		/// </summary>
		public string Prefix { get; set; } = string.Empty;

		/// <summary>
		/// To provide a suffix to add at the end of the translated text.
		/// </summary>
		public string Suffix { get; set; } = string.Empty;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Prefix + Loc.Tr(TextId, DefaultText?.Replace("[apos]", "'"), value?.ToString()) + Suffix;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
