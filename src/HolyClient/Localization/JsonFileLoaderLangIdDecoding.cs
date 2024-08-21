namespace HolyClient.Localization;

/// <summary>
///     Specify the way a <see cref="JsonFileLoader" /> decode the LangId
/// </summary>
public enum JsonFileLoaderLangIdDecoding
{
	/// <summary>
	///     Default value. The JSON leaf node is interpreted as the LangId
	/// </summary>
	LeafNodeKey,

	/// <summary>
	///     Take the part of the filename just before .loc.json as the LangId
	///     <para>
	///         Example : MyTranslationFile.en.loc.json
	///     </para>
	/// </summary>
	InFileNameBeforeExtension,

	/// <summary>
	///     The directory name define the LangId
	/// </summary>
	DirectoryName
}