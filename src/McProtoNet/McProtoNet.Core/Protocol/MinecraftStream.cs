using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace McProtoNet.Core.Protocol
{
	public sealed class MinecraftStream : Stream, IDisposable
	{

		public bool EncryptionEnabled { get; private set; } = false;


		internal Stream BaseStream;

		public override bool CanRead => BaseStream.CanRead;

		public override bool CanSeek => BaseStream.CanSeek;

		public override bool CanWrite => BaseStream.CanWrite;

		public override long Length => BaseStream.Length;

		public override long Position { get => BaseStream.Position; set => BaseStream.Position = value; }
		

		public MinecraftStream(Stream stream)
		{
#if NETSTANDARD2_0
            if(stream is null)
                throw new ArgumentNullException(nameof(stream));
#else
			ArgumentNullException.ThrowIfNull(stream, nameof(stream));
#endif
			this.BaseStream = stream;
		}


		public int ReadVarInt()
		{
			return BaseStream.ReadVarInt();
		}


		public async ValueTask<int> ReadVarIntAsync(CancellationToken token = default)
		{
			int numRead = 0;
			int result = 0;
			byte read;
			do
			{
				token.ThrowIfCancellationRequested();
				read = await this.ReadUnsignedByteAsync(token);

				int value = read & 0b01111111;
				result |= value << (7 * numRead);

				numRead++;
				if (numRead > 5)
				{
					throw new InvalidOperationException("VarInt is too big");
				}
			} while ((read & 0b10000000) != 0);

			return result;

		}
		public async ValueTask<(int, int)> ReadVarIntAndLenAsync(CancellationToken token = default)
		{
			int numRead = 0;
			int result = 0;
			byte read;
			do
			{
				token.ThrowIfCancellationRequested();
				read = await this.ReadUnsignedByteAsync(token);

				int value = read & 0b01111111;
				result |= value << (7 * numRead);

				numRead++;
				if (numRead > 5)
				{
					throw new InvalidOperationException("VarInt is too big");
				}
			} while ((read & 0b10000000) != 0);

			return (result, numRead);

		}




		#region Приватные

		private byte ReadUnsignedByte()
		{
			int b = ReadByte();
			if (b == -1)
				throw new InvalidOperationException("Stream end");
			return (byte)b;
		}
		private void WriteUnsignedByte(byte val)
		{
			WriteByte(val);
		}

		private async ValueTask<byte> ReadUnsignedByteAsync(CancellationToken token = default)
		{
			token.ThrowIfCancellationRequested();
			var buffer = new byte[1];
			await this.ReadAsync(buffer, token);
			return buffer[0];

		}
		private async ValueTask WriteUnsignedByteAsync(byte value, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			await WriteAsync(new[] { value }, token);
		}
		private IBufferedCipher EncryptCipher { get; set; }
		private IBufferedCipher DecryptCipher { get; set; }
		#endregion
		/// <summary>
		/// Включает AES шифрование
		/// </summary>
		/// <param name="privatekey">Ключ</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void SwitchEncryption(byte[] privatekey)
		{
			if (EncryptionEnabled)
			{
				throw new InvalidOperationException("Шифрование уже включено");
			}

			EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
			EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(privatekey), privatekey, 0, 16));

			DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
			DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(privatekey), privatekey, 0, 16));

			this.BaseStream = new AsyncCipherStream(BaseStream, DecryptCipher, EncryptCipher);
		}
		public override int Read(Span<byte> buffer)
		{
			return BaseStream.Read(buffer);
		}
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			BaseStream.Write(buffer);
		}
		public override void CopyTo(Stream destination, int bufferSize)
		{
			BaseStream.CopyTo(destination, bufferSize);
		}
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			return BaseStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}
		public override int ReadByte()
		{
			return BaseStream.ReadByte();
		}
		public override int EndRead(IAsyncResult asyncResult)
		{
			return BaseStream.EndRead(asyncResult);
		}
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		{
			return BaseStream.BeginRead(buffer, offset, count, callback, state);
		}
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		{
			return BaseStream.BeginWrite(buffer, offset, count, callback, state);
		}
		public override void EndWrite(IAsyncResult asyncResult)
		{
			BaseStream.EndWrite(asyncResult);
		}

		public override bool Equals(object? obj)
		{
			return BaseStream.Equals(obj);
		}
		public override int GetHashCode()
		{
			return BaseStream.GetHashCode();
		}
		public override void Flush()
		{
			BaseStream.Flush();
		}
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return BaseStream.FlushAsync(cancellationToken);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return BaseStream.Read(buffer, offset, count);
		}
		/// <summary>
		/// asd
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return BaseStream.ReadAsync(buffer, cancellationToken);
		}
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return BaseStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return BaseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			BaseStream.SetLength(value);
		}
		public override void WriteByte(byte value)
		{
			BaseStream.WriteByte(value);
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			BaseStream.Write(buffer, offset, count);
		}
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return BaseStream.WriteAsync(buffer, offset, count, cancellationToken);
		}
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return BaseStream.WriteAsync(buffer, cancellationToken);
		}
		private bool _disposed;
		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			BaseStream.Dispose();
			
			_disposed = true;
		}

	}

}


