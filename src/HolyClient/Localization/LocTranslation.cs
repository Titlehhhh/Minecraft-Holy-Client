using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HolyClient.Localization;

/// <summary>
///     Represent a single translation. used to keep track of the link between TextId, LanguageId, the Source and
/// </summary>
public class LocTranslation : INotifyPropertyChanged
{
	/// <summary>
	///     The source from where comes this translation (filename or id)
	/// </summary>
	public string Source { get; set; }

	/// <summary>
	///     Id of the translation (label)
	/// </summary>
	public string TextId { get; set; }

	/// <summary>
	///     The language to which the text is translated
	/// </summary>
	public string LanguageId { get; set; }

	/// <summary>
	///     The translated text
	/// </summary>
	public string TranslatedText { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}