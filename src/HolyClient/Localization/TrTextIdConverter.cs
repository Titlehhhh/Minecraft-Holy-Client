using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HolyClient.Localization
{
	/// <summary>
	/// Converter to Translate a the binding textId (In CurrentLanguage or if specified in LanguageId)
	/// If Translation don't exist return DefaultText
	/// Not usable in TwoWay Binding mode.
	/// </summary>
	public class TrTextIdConverter : IValueConverter
	{
		public TrTextIdConverter()
		{
			//WeakEventManager<Loc, CurrentLanguageChangedEventArgs>.AddHandler(Loc.Instance, nameof(Loc.Instance.CurrentLanguageChanged), CurrentLanguageChanged);
		}

		~TrTextIdConverter()
		{
			//WeakEventManager<Loc, CurrentLanguageChangedEventArgs>.RemoveHandler(Loc.Instance, nameof(Loc.Instance.CurrentLanguageChanged), CurrentLanguageChanged);
		}

		/// <summary>
		/// The text to return if no text correspond to textId in the current language
		/// </summary>
		public string DefaultText { get; set; }

		/// <summary>
		/// The language id in which to get the translation. To Specify if not CurrentLanguage
		/// </summary>
		public string LanguageId { get; set; }

		/// <summary>
		/// A string format where will be injected the binding
		/// by Default => {0}
		/// </summary>
		public string TextIdStringFormat { get; set; } = "{0}";

		/// <summary>
		/// AllowTo use a converter on the binded value before to inject it in the TextId
		/// </summary>
		public IValueConverter PreConverter { get; set; }

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
			string textId = PreConverter?.Convert(value, null, null, null)?.ToString() ?? value?.ToString();
			return Prefix + (string.IsNullOrEmpty(textId) ? "" : Loc.Tr(string.Format(TextIdStringFormat, textId), DefaultText?.Replace("[apos]", "'"), LanguageId)) + Suffix;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			//SetXamlObjects(serviceProvider);

			return this;
		}

		private void CurrentLanguageChanged(object sender, CurrentLanguageChangedEventArgs e)
		{
			//if (xamlTargetObject != null && xamlDependencyProperty != null)
			//{
			//    if(IsInAMultiBinding)
			//        BindingOperations.GetMultiBindingExpression(xamlTargetObject, xamlDependencyProperty)?.UpdateTarget();
			//    else
			//        BindingOperations.GetBindingExpression(xamlTargetObject, xamlDependencyProperty)?.UpdateTarget();
			//}
		}
	}
}
