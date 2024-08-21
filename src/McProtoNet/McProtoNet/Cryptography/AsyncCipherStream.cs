using System.Diagnostics;
using Org.BouncyCastle.Crypto;

namespace McProtoNet.Cryptography;

public class AsyncCipherStream : Stream
{
    internal Stream stream;
    internal IBufferedCipher inCipher, outCipher;
    private byte[] mInBuf;
    private int mInPos;
    private bool inStreamEnded;

    public AsyncCipherStream(
        Stream stream,
        IBufferedCipher readCipher,
        IBufferedCipher writeCipher)
    {
        this.stream = stream;

        if (readCipher != null)
        {
            inCipher = readCipher;
            mInBuf = null;
        }

        if (writeCipher != null) outCipher = writeCipher;
    }

    public IBufferedCipher ReadCipher => inCipher;

    public IBufferedCipher WriteCipher => outCipher;

    public override int ReadByte()
    {
        if (inCipher == null)
            return stream.ReadByte();

        if (mInBuf == null || mInPos >= mInBuf.Length)
            if (!FillInBuf())
                return -1;

        return mInBuf[mInPos++];
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count,
        CancellationToken cancellationToken)
    {
        if (inCipher == null)
            return await stream.ReadAsync(buffer, offset, count, cancellationToken);

        var num = 0;
        while (num < count && !cancellationToken.IsCancellationRequested)
        {
            if (mInBuf == null || mInPos >= mInBuf.Length)
                if (!await FillInBufAsync(cancellationToken))
                    break;

            var numToCopy = Math.Min(count - num, mInBuf.Length - mInPos);
            Array.Copy(mInBuf, mInPos, buffer, offset + num, numToCopy);
            mInPos += numToCopy;
            num += numToCopy;
        }

        return num;
    }

    public override int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        if (inCipher == null)
            return stream.Read(buffer, offset, count);

        var num = 0;
        while (num < count)
        {
            if (mInBuf == null || mInPos >= mInBuf.Length)
                if (!FillInBuf())
                    break;

            var numToCopy = Math.Min(count - num, mInBuf.Length - mInPos);
            Array.Copy(mInBuf, mInPos, buffer, offset + num, numToCopy);
            mInPos += numToCopy;
            num += numToCopy;
        }

        return num;
    }

    private async ValueTask<bool> FillInBufAsync(CancellationToken cancellation)
    {
        if (inStreamEnded)
            return false;
        mInPos = 0;

        do
        {
            mInBuf = await ReadAndProcessBlockAsync(cancellation);
        } while (!inStreamEnded && mInBuf == null && !cancellation.IsCancellationRequested);

        return mInBuf != null;
    }

    private bool FillInBuf()
    {
        if (inStreamEnded)
            return false;

        mInPos = 0;

        do
        {
            mInBuf = ReadAndProcessBlock();
        } while (!inStreamEnded && mInBuf == null);

        return mInBuf != null;
    }

    private async Task<byte[]> ReadAndProcessBlockAsync(CancellationToken cancellation)
    {
        var blockSize = inCipher.GetBlockSize();
        var readSize = blockSize == 0 ? 256 : blockSize;

        var block = new byte[readSize];
        var numRead = 0;
        do
        {
            var count = await stream.ReadAsync(block, numRead, block.Length - numRead, cancellation);
            if (count <= 0)
            {
                //throw new EndOfStreamException();
                inStreamEnded = true;
                break;
            }

            numRead += count;
        } while (numRead < block.Length && !cancellation.IsCancellationRequested);

        Debug.Assert(inStreamEnded || numRead == block.Length);

        var bytes = inStreamEnded
            ? inCipher.DoFinal(block, 0, numRead)
            : inCipher.ProcessBytes(block);

        if (bytes != null && bytes.Length == 0) bytes = null;

        return bytes;
    }

    private byte[] ReadAndProcessBlock()
    {
        var blockSize = inCipher.GetBlockSize();
        var readSize = blockSize == 0 ? 256 : blockSize;

        var block = new byte[readSize];
        var numRead = 0;
        do
        {
            var count = stream.Read(block, numRead, block.Length - numRead);
            if (count < 1)
            {
                inStreamEnded = true;
                break;
            }

            numRead += count;
        } while (numRead < block.Length);

        Debug.Assert(inStreamEnded || numRead == block.Length);

        var bytes = inStreamEnded
            ? inCipher.DoFinal(block, 0, numRead)
            : inCipher.ProcessBytes(block);

        if (bytes != null && bytes.Length == 0) bytes = null;

        return bytes;
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var data = outCipher.ProcessBytes(buffer, offset, count);
        if (data != null) return stream.WriteAsync(data, 0, data.Length, cancellationToken);

        return Task.CompletedTask;
    }

    public override void Write(
        byte[] buffer,
        int offset,
        int count)
    {
        Debug.Assert(buffer != null);
        Debug.Assert(0 <= offset && offset <= buffer.Length);
        Debug.Assert(count >= 0);

        var end = offset + count;

        Debug.Assert(0 <= end && end <= buffer.Length);

        if (outCipher == null)
        {
            stream.Write(buffer, offset, count);
            return;
        }

        var data = outCipher.ProcessBytes(buffer, offset, count);
        if (data != null) stream.Write(data, 0, data.Length);
    }

    public override void WriteByte(
        byte b)
    {
        if (outCipher == null)
        {
            stream.WriteByte(b);
            return;
        }

        var data = outCipher.ProcessByte(b);
        if (data != null) stream.Write(data, 0, data.Length);
    }

    public override bool CanRead => stream.CanRead && inCipher != null;

    public override bool CanWrite => stream.CanWrite && outCipher != null;

    public override bool CanSeek => false;

    public sealed override long Length => throw new NotSupportedException();

    public sealed override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

#if PORTABLE
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
			    if (outCipher != null)
			    {
				    byte[] data = outCipher.DoFinal();
				    stream.Write(data, 0, data.Length);
				    stream.Flush();
			    }
                Platform.Dispose(stream);
            }
            base.Dispose(disposing);
        }
#else
    public override void Close()
    {
        if (outCipher != null)
        {
            var data = outCipher.DoFinal();
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        base.Close();
    }
#endif

    public override void Flush()
    {
        // Note: outCipher.DoFinal is only called during Close()
        stream.Flush();
    }

    public sealed override long Seek(
        long offset,
        SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public sealed override void SetLength(
        long length)
    {
        throw new NotSupportedException();
    }
}