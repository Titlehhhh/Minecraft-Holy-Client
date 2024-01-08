using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using McProtoNet.Utils;
using Microsoft.IO;
using QuickProxyNet;
using Serilog;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Reactive.Subjects;

namespace McProtoNet
{
	
	public class ReadWriteStream : Stream
	{
		private readonly Stream _write;

		private readonly Stream _read;
		public ReadWriteStream(Stream write, Stream read)
		{
			_write = write;
			_read = read;
		}


		public override bool CanRead => throw new NotImplementedException();

		public override bool CanSeek => throw new NotImplementedException();

		public override bool CanWrite => throw new NotImplementedException();

		public override long Length => throw new NotImplementedException();

		public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void Flush()
		{
			_write.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _read.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_write.Write(buffer, offset, count);
		}

		public override void Write(ReadOnlySpan<byte> buffer)
		{
			_write.Write(buffer);
		}
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return _write.WriteAsync(buffer, offset, count, cancellationToken);
		}
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return _write.WriteAsync(buffer, cancellationToken);
		}
		public override int Read(Span<byte> buffer)
		{
			return base.Read(buffer);
		}
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return _write.FlushAsync(cancellationToken);
		}
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			return _read.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return _read.ReadAsync(buffer, offset, count, cancellationToken);
		}
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return _read.ReadAsync(buffer, cancellationToken);
		}

		public override int ReadByte()
		{
			return _read.ReadByte();
		}

		public override int ReadTimeout { get => _read.ReadTimeout; set => _read.ReadTimeout = value; }

		public override int WriteTimeout { get => _write.WriteTimeout; set => _write.WriteTimeout = value; }


		public override void WriteByte(byte value)
		{
			_write.WriteByte(value);
		}

		protected override void Dispose(bool disposing)
		{

			_read.Dispose();
			_write.Dispose();
			base.Dispose(disposing);
		}


	}
}