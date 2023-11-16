using System.IO.Compression;

namespace McProtoNet.NBT
{
	/// <summary>
	/// DeflateStream wrapper that calculates Adler32 checksum of the written data,
	/// to allow writing ZLib header (RFC-1950).
	/// </summary>
	// ReSharper disable once InconsistentNaming
	internal sealed class ZLibStream : DeflateStream
	{
		private int _adler32A = 1,
			_adler32B;

		private const int ChecksumModulus = 65521;

		public int Checksum => unchecked(_adler32B * 65536 + _adler32A);

		private void UpdateChecksum(IList<byte> data, int offset, int length)
		{
			for (int counter = 0; counter < length; ++counter)
			{
				_adler32A = (_adler32A + (data[offset + counter])) % ChecksumModulus;
				_adler32B = (_adler32B + _adler32A) % ChecksumModulus;
			}
		}

		public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: base(stream, mode, leaveOpen) { }

		public override void Write(byte[] array, int offset, int count)
		{
			UpdateChecksum(array, offset, count);
			base.Write(array, offset, count);
		}
	}
}