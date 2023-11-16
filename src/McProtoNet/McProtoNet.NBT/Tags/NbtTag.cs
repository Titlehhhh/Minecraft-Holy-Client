using System.Globalization;
using System.Text;

namespace McProtoNet.NBT
{
	/// <summary>
	/// Base class for different kinds of named binary tags.
	/// </summary>
	public abstract class NbtTag : ICloneable
	{

		/// <summary>
		/// Parent compound tag, either NbtList or NbtCompound, if any.
		/// May be <c>null</c> for detached tags.
		/// </summary>
		public NbtTag? Parent { get; internal set; }

		/// <summary>
		/// Type of this tag.
		/// </summary>
		public abstract NbtTagType TagType { get; }

		/// <summary>
		/// Returns true if tags of this type have a value attached.
		/// All tags except Compound, List, and End have values.
		/// </summary>
		public bool HasValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Compound => false,
					NbtTagType.End => false,
					NbtTagType.List => false,
					NbtTagType.Unknown => false,
					_ => true
				};
			}
		}

		/// <summary>
		/// Name of this tag. Immutable, and set by the constructor. May be <c>null</c>.
		/// </summary>
		/// <exception cref="ArgumentNullException"> If <paramref name="value"/> is <c>null</c>, and <c>Parent</c> tag is an NbtCompound.
		/// Name of tags inside an <c>NbtCompound</c> may not be null. </exception>
		/// <exception cref="ArgumentException"> If this tag resides in an <c>NbtCompound</c>, and a sibling tag with the name already exists. </exception>
		public string? Name
		{
			get => StrName;
			set
			{
				if (StrName == value)
				{
					return;
				}

				if (Parent is NbtCompound parentAsCompound)
				{
					if (value == null)
					{
						throw new ArgumentNullException(nameof(value),
														"Name of tags inside an NbtCompound may not be null.");
					}

					if (StrName != null)
					{
						parentAsCompound.RenameTag(StrName, value);
					}
				}

				StrName = value;
			}
		}

		/// <summary>
		/// Used by impls to bypass setter checks (and avoid side effects) when initializing state
		/// </summary>
		internal string? StrName;

		/// <summary>
		/// Gets the full name of this tag, including all parent tag names, separated by dots. 
		/// Unnamed tags show up as empty strings.
		/// </summary>
		public string Path
		{
			get
			{
				if (Parent == null)
				{
					return Name ?? "";
				}
				if (Parent is NbtList parentAsList)
				{
					return parentAsList.Path + '[' + parentAsList.IndexOf(this) + ']';
				}

				return Parent.Path + '.' + Name;
			}
		}

		internal abstract bool ReadTag(NbtBinaryReader readStream);

		internal abstract void SkipTag(NbtBinaryReader readStream);

		internal abstract void WriteTag(NbtBinaryWriter writeReader);

		/// <summary>
		/// WriteData does not write the tag's ID byte or the name
		/// </summary>
		internal abstract void WriteData(NbtBinaryWriter writeStream);

		#region Shortcuts

		/// <summary>
		/// Gets or sets the tag with the specified name. May return <c>null</c>.
		/// </summary>
		/// <returns> The tag with the specified key. Null if tag with the given name was not found. </returns>
		/// <param name="tagName"> The name of the tag to get or set. Must match tag's actual name. </param>
		/// <exception cref="InvalidOperationException"> If used on a tag that is not NbtCompound. </exception>
		/// <remarks> ONLY APPLICABLE TO NbtCompound OBJECTS!
		/// Included in NbtTag base class for programmers' convenience, to avoid extra type casts. </remarks>
		public virtual NbtTag? this[string tagName]
		{
			get => throw new InvalidOperationException("String indexers only work on NbtCompound tags.");
			set => throw new InvalidOperationException("String indexers only work on NbtCompound tags.");
		}

		/// <summary>
		/// Gets or sets the tag at the specified index.
		/// </summary>
		/// <returns> The tag at the specified index. </returns>
		/// <param name="tagIndex"> The zero-based index of the tag to get or set. </param>
		/// <exception cref="ArgumentOutOfRangeException"> tagIndex is not a valid index in this tag. </exception>
		/// <exception cref="ArgumentNullException"> Given tag is <c>null</c>. </exception>
		/// <exception cref="ArgumentException"> Given tag's type does not match ListType. </exception>
		/// <exception cref="InvalidOperationException"> If used on a tag that is not NbtList, NbtByteArray, or NbtIntArray. </exception>
		/// <remarks> ONLY APPLICABLE TO NbtList, NbtByteArray, and NbtIntArray OBJECTS!
		/// Included in NbtTag base class for programmers' convenience, to avoid extra type casts. </remarks>
		public virtual NbtTag this[int tagIndex]
		{
			get => throw new InvalidOperationException("Integer indexers only work on NbtList tags.");
			set => throw new InvalidOperationException("Integer indexers only work on NbtList tags.");
		}

		public bool BoolValue => this.ByteValue != 0;

		/// <summary>
		/// Returns the value of this tag, cast as a byte.
		/// Only supported by NbtByte tags.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on a tag other than NbtByte. </exception>
		public byte ByteValue
		{
			get
			{
				if (TagType == NbtTagType.Byte)
				{
					return ((NbtByte)this).Value;
				}

				throw new InvalidCastException("Cannot get ByteValue from " + GetCanonicalTagName(TagType));
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a short (16-bit signed integer).
		/// Only supported by NbtByte and NbtShort.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public short ShortValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Byte => ((NbtByte)this).Value,
					NbtTagType.Short => ((NbtShort)this).Value,
					_ => throw new InvalidCastException("Cannot get ShortValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as an int (32-bit signed integer).
		/// Only supported by NbtByte, NbtShort, and NbtInt.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public int IntValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Byte => ((NbtByte)this).Value,
					NbtTagType.Short => ((NbtShort)this).Value,
					NbtTagType.Int => ((NbtInt)this).Value,
					_ => throw new InvalidCastException("Cannot get IntValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a long (64-bit signed integer).
		/// Only supported by NbtByte, NbtShort, NbtInt, and NbtLong.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public long LongValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Byte => ((NbtByte)this).Value,
					NbtTagType.Short => ((NbtShort)this).Value,
					NbtTagType.Int => ((NbtInt)this).Value,
					NbtTagType.Long => ((NbtLong)this).Value,
					_ => throw new InvalidCastException("Cannot get LongValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a long (64-bit signed integer).
		/// Only supported by NbtFloat and, with loss of precision, by NbtDouble, NbtByte, NbtShort, NbtInt, and NbtLong.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public float FloatValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Byte => ((NbtByte)this).Value,
					NbtTagType.Short => ((NbtShort)this).Value,
					NbtTagType.Int => ((NbtInt)this).Value,
					NbtTagType.Long => ((NbtLong)this).Value,
					NbtTagType.Float => ((NbtFloat)this).Value,
					NbtTagType.Double => (float)((NbtDouble)this).Value,
					_ => throw new InvalidCastException("Cannot get FloatValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a long (64-bit signed integer).
		/// Only supported by NbtFloat, NbtDouble, and, with loss of precision, by NbtByte, NbtShort, NbtInt, and NbtLong.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public double DoubleValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.Byte => ((NbtByte)this).Value,
					NbtTagType.Short => ((NbtShort)this).Value,
					NbtTagType.Int => ((NbtInt)this).Value,
					NbtTagType.Long => ((NbtLong)this).Value,
					NbtTagType.Float => ((NbtFloat)this).Value,
					NbtTagType.Double => ((NbtDouble)this).Value,
					_ => throw new InvalidCastException("Cannot get DoubleValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a byte array.
		/// Only supported by NbtByteArray tags.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on a tag other than NbtByteArray. </exception>
		public byte[] ByteArrayValue
		{
			get
			{
				if (TagType == NbtTagType.ByteArray)
				{
					return ((NbtByteArray)this).Value;
				}

				throw new InvalidCastException("Cannot get ByteArrayValue from " + GetCanonicalTagName(TagType));
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as an int array.
		/// Only supported by NbtIntArray tags.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on a tag other than NbtIntArray. </exception>
		public int[] IntArrayValue
		{
			get
			{
				if (TagType == NbtTagType.IntArray)
				{
					return ((NbtIntArray)this).Value;
				}

				throw new InvalidCastException("Cannot get IntArrayValue from " + GetCanonicalTagName(TagType));
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a long array.
		/// Only supported by NbtLongArray tags.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on a tag other than NbtLongArray. </exception>
		public long[] LongArrayValue
		{
			get
			{
				if (TagType == NbtTagType.LongArray)
				{
					return ((NbtLongArray)this).Value;
				}

				throw new InvalidCastException("Cannot get LongArrayValue from " + GetCanonicalTagName(TagType));
			}
		}

		/// <summary>
		/// Returns the value of this tag, cast as a string.
		/// Returns exact value for NbtString, and stringified (using InvariantCulture) value for NbtByte, NbtDouble, NbtFloat, NbtInt, NbtLong, and NbtShort.
		/// Not supported by NbtCompound, NbtList, NbtByteArray, or NbtIntArray.
		/// </summary>
		/// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
		public string StringValue
		{
			get
			{
				return TagType switch
				{
					NbtTagType.String => ((NbtString)this).Value,
					NbtTagType.Byte => ((NbtByte)this).Value.ToString(CultureInfo.InvariantCulture),
					NbtTagType.Double => ((NbtDouble)this).Value.ToString(CultureInfo.InvariantCulture),
					NbtTagType.Float => ((NbtFloat)this).Value.ToString(CultureInfo.InvariantCulture),
					NbtTagType.Int => ((NbtInt)this).Value.ToString(CultureInfo.InvariantCulture),
					NbtTagType.Long => ((NbtLong)this).Value.ToString(CultureInfo.InvariantCulture),
					NbtTagType.Short => ((NbtShort)this).Value.ToString(CultureInfo.InvariantCulture),
					_ => throw new InvalidCastException("Cannot get StringValue from " + GetCanonicalTagName(TagType))
				};
			}
		}

		#endregion

		/// <summary>
		/// Returns a canonical (Notchy) name for the given NbtTagType,
		/// e.g. "TAG_Byte_Array" for NbtTagType.ByteArray
		/// </summary>
		/// <param name="type"> NbtTagType to name. </param>
		/// <returns> String representing the canonical name of a tag,
		/// or null of given TagType does not have a canonical name (e.g. Unknown) </returns>
		public static string? GetCanonicalTagName(NbtTagType type)
		{
			return type switch
			{
				NbtTagType.Byte => "TAG_Byte",
				NbtTagType.ByteArray => "TAG_Byte_Array",
				NbtTagType.Compound => "TAG_Compound",
				NbtTagType.Double => "TAG_Double",
				NbtTagType.End => "TAG_End",
				NbtTagType.Float => "TAG_Float",
				NbtTagType.Int => "TAG_Int",
				NbtTagType.IntArray => "TAG_Int_Array",
				NbtTagType.LongArray => "TAG_Long_Array",
				NbtTagType.List => "TAG_List",
				NbtTagType.Long => "TAG_Long",
				NbtTagType.Short => "TAG_Short",
				NbtTagType.String => "TAG_String",
				_ => null
			};
		}

		/// <summary>
		/// Prints contents of this tag, and any child tags, to a string.
		/// Indents the string using multiples of the given indentation string.
		/// </summary>
		/// <returns> A string representing contents of this tag, and all child tags (if any). </returns>
		public override string ToString()
		{
			return ToString(DefaultIndentString);
		}

		/// <summary>
		/// Creates a deep copy of this tag.
		/// </summary>
		/// <returns> A new NbtTag object that is a deep copy of this instance. </returns>
		public abstract object Clone();

		/// <summary>
		/// Prints contents of this tag, and any child tags, to a string.
		/// Indents the string using multiples of the given indentation string.
		/// </summary>
		/// <param name="indentString"> String to be used for indentation. </param>
		/// <returns> A string representing contents of this tag, and all child tags (if any). </returns>
		/// <exception cref="ArgumentNullException"> <paramref name="indentString"/> is <c>null</c>. </exception>
		public string ToString(string indentString)
		{
			if (indentString == null) throw new ArgumentNullException(nameof(indentString));
			var sb = new StringBuilder();
			PrettyPrint(sb, indentString, 0);
			return sb.ToString();
		}

		internal abstract void PrettyPrint(StringBuilder sb, string indentString, int indentLevel);

		/// <summary>
		/// String to use for indentation in NbtTag's and NbtFile's ToString() methods by default.
		/// </summary>
		/// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
		public static string DefaultIndentString
		{
			get => _defaultIndentString;
			set => _defaultIndentString = value ?? throw new ArgumentNullException(nameof(value));
		}

		private static string _defaultIndentString = "  ";
	}
}
