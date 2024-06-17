using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace McProtoNet.Cryptography;

public sealed class AesStream : Stream, IDisposable
{
    private bool _disposed;


    internal Stream BaseStream;


    public AesStream(Stream stream)
    {
#if NETSTANDARD2_0
            if(stream is null)
                throw new ArgumentNullException(nameof(stream));
#else
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
#endif
        BaseStream = stream;
    }

    public bool EncryptionEnabled { get; } = false;

    public override bool CanRead => BaseStream.CanRead;

    public override bool CanSeek => BaseStream.CanSeek;

    public override bool CanWrite => BaseStream.CanWrite;

    public override long Length => BaseStream.Length;

    public override long Position
    {
        get => BaseStream.Position;
        set => BaseStream.Position = value;
    }


    private IBufferedCipher EncryptCipher { get; set; }
    private IBufferedCipher DecryptCipher { get; set; }


    public int ReadVarInt()
    {
        return BaseStream.ReadVarInt();
    }

    /// <summary>
    ///     Включает AES шифрование
    /// </summary>
    /// <param name="privatekey">Ключ</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SwitchEncryption(byte[] privatekey)
    {
        if (EncryptionEnabled) throw new InvalidOperationException("Шифрование уже включено");

        EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
        EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(privatekey), privatekey, 0, 16));

        DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
        DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(privatekey), privatekey, 0, 16));

        BaseStream = new AsyncCipherStream(BaseStream, DecryptCipher, EncryptCipher);
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

    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback,
        object? state)
    {
        return BaseStream.BeginRead(buffer, offset, count, callback, state);
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback,
        object? state)
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
    ///     asd
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

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        BaseStream.Dispose();

        _disposed = true;
    }
}