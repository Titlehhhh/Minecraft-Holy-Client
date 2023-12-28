using Microsoft.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketSender : IMinecraftPacketSender
	{
		private Stream _baseStream;
		private readonly bool disposedStream;
		public MinecraftPacketSender(Stream baseStream, bool disposedStream)
		{
			_baseStream = baseStream;
			this.disposedStream = disposedStream;
		}
		public MinecraftPacketSender(Stream baseStream) : this(baseStream, true)
		{

		}

		private const int ZERO_VARLENGTH = 1;//default(int).GetVarIntLength();
		private readonly byte[] ZERO_VARINT = { 0 };
		private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

		
		public void SendPacket(Packet packet)
		{
			semaphore.Wait();
			int id = packet.Id;
			var data = packet.Data;
			try
			{
				ThrowIfDisposed();




				if (_compressionThreshold > 0)
				{
					Span<byte> idData = stackalloc byte[5];

					int idLen = id.GetVarIntLength(idData);

					//Длина ID+DATA
					int uncompressedSize = idLen + (int)data.Length;

					if (uncompressedSize >= _compressionThreshold)
					{

						using (var compressedPacket = StaticResources.MSmanager.GetStream())
						{
							using (var zlibStream = new ZLibStream(compressedPacket, CompressionMode.Compress, true))
							{
								zlibStream.WriteVarInt(id);
								data.CopyTo(zlibStream);
							}
							int uncompressedSizeLength = uncompressedSize.GetVarIntLength();

							int fullSize = uncompressedSizeLength + (int)compressedPacket.Length;

							_baseStream.WriteVarInt(fullSize);

							_baseStream.WriteVarInt(uncompressedSize);

							compressedPacket.Position = 0;
							compressedPacket.CopyTo(_baseStream);

						}

					}
					else
					{
						#region Short                    
						uncompressedSize++;

						_baseStream.WriteVarInt(uncompressedSize);

						_baseStream.Write(ZERO_VARINT);

						_baseStream.Write(idData.Slice(0, idLen));

						data.CopyTo(_baseStream);
						#endregion
					}
				}
				else
				{
					SendPacketWithoutCompression(packet.Data, id);
				}
				_baseStream.Flush();
			}
			finally
			{
				semaphore.Release();
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private void SendPacketWithoutCompression(MemoryStream packet, int id)
		{

			ThrowIfDisposed();
			// packet.Write(writer);
			int Packetlength = (int)packet.Length;


			Packetlength += id.GetVarIntLength();

			//Записываем длину всего пакета           
			_baseStream.WriteVarInt(Packetlength);
			//Записываем ID пакета
			_baseStream.WriteVarInt(id);

			//Все данные пакета перекидваем в интернет
			packet.CopyTo(_baseStream);


		}

		#region Send        
		public async ValueTask SendPacketAsync(Packet packet, CancellationToken token = default)
		{

			ThrowIfDisposed();
			int id = packet.Id;
			var data = packet.Data;
			//await semaphore.WaitAsync(token);
			try
			{


				if (_compressionThreshold > 0)
				{


					byte[] idData = new byte[5];

					int idLen = id.GetVarIntLength(idData);


					int uncompressedSize = idLen + (int)data.Length;
					if (uncompressedSize >= _compressionThreshold)
					{

						using (var compressedPacket = StaticResources.MSmanager.GetStream())
						{
							using (var zlibStream = new ZLibStream(compressedPacket, CompressionMode.Compress, true))
							{
								await zlibStream.WriteVarIntAsync(id);
								await data.CopyToAsync(zlibStream, token);
							}
							int uncompressedSizeLength = uncompressedSize.GetVarIntLength();

							int fullSize = uncompressedSizeLength + (int)compressedPacket.Length;



							await _baseStream.WriteVarIntAsync(fullSize, token);

							await _baseStream.WriteVarIntAsync(uncompressedSize, token);

							compressedPacket.Position = 0;
							await compressedPacket.CopyToAsync(_baseStream, token);

						}
					}
					else
					{
						uncompressedSize++;

						await _baseStream.WriteVarIntAsync(uncompressedSize, token);
						await _baseStream.WriteAsync(ZERO_VARINT, token);
						await _baseStream.WriteAsync(idData.AsMemory(0, idLen), token);

						await data.CopyToAsync(_baseStream);


					}
				}
				else
				{
					await SendPacketWithoutCompressionAsync(packet.Data, id, token);
				}
				await _baseStream.FlushAsync(token);
			}
			finally
			{
				//semaphore.Release();
			}
		}

		private async Task SendPacketWithoutCompressionAsync(MemoryStream packet, int id, CancellationToken token)
		{
			ThrowIfDisposed();
			// packet.Write(writer);
			int Packetlength = (int)packet.Length;

			byte[] idData = new byte[5];
			int len = id.GetVarIntLength(idData);
			//Записываем длину всего пакета
			await _baseStream.WriteVarIntAsync(Packetlength + len, token);
			//Записываем ID пакета
			await _baseStream.WriteAsync(idData, 0, len, token);


			//Все данные пакета перекидваем в интернет
			await packet.CopyToAsync(_baseStream, token);


		}
		#endregion
		~MinecraftPacketSender()
		{
			Dispose();
		}
		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}

		private bool _disposed;
		private void ThrowIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(MinecraftPacketSender));
		}

		public void Dispose()
		{

			if (_disposed)
				return;
			if (semaphore is not null)
			{
				semaphore.Dispose();
				semaphore = null;
			}
			if (disposedStream)
			{
				if (_baseStream is { })
				{
					_baseStream.Dispose();
					_baseStream = null;
				}
			}
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			if (_disposed)
				_disposed = true;
			if (semaphore is not null)
			{
				semaphore.Dispose();
				semaphore = null;
			}
			if (disposedStream)
			{
				if (_baseStream is not null)
				{
					await _baseStream.DisposeAsync();
					_baseStream = null;
				}
			}
			GC.SuppressFinalize(this);
		}
	}
}


