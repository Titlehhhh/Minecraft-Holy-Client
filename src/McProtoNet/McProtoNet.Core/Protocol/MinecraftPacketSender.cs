using Microsoft.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketSender : IMinecraftPacketSender
	{
		public Stream BaseStream { get; set; }

		public MinecraftPacketSender(Stream baseStream)
		{
			BaseStream = baseStream;			
		}
		public MinecraftPacketSender()
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

							BaseStream.WriteVarInt(fullSize);

							BaseStream.WriteVarInt(uncompressedSize);

							compressedPacket.Position = 0;
							compressedPacket.CopyTo(BaseStream);

						}

					}
					else
					{
						#region Short                    
						uncompressedSize++;

						BaseStream.WriteVarInt(uncompressedSize);

						BaseStream.Write(ZERO_VARINT);

						BaseStream.Write(idData.Slice(0, idLen));

						data.CopyTo(BaseStream);
						#endregion
					}
				}
				else
				{
					SendPacketWithoutCompression(packet.Data, id);
				}
				BaseStream.Flush();
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
			BaseStream.WriteVarInt(Packetlength);
			//Записываем ID пакета
			BaseStream.WriteVarInt(id);

			//Все данные пакета перекидваем в интернет
			packet.CopyTo(BaseStream);


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



							await BaseStream.WriteVarIntAsync(fullSize, token);

							await BaseStream.WriteVarIntAsync(uncompressedSize, token);

							compressedPacket.Position = 0;
							await compressedPacket.CopyToAsync(BaseStream, token);

						}
					}
					else
					{
						uncompressedSize++;

						await BaseStream.WriteVarIntAsync(uncompressedSize, token);
						await BaseStream.WriteAsync(ZERO_VARINT, token);
						await BaseStream.WriteAsync(idData.AsMemory(0, idLen), token);

						await data.CopyToAsync(BaseStream);


					}
				}
				else
				{
					await SendPacketWithoutCompressionAsync(packet.Data, id, token);
				}
				await BaseStream.FlushAsync(token);
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
			await BaseStream.WriteVarIntAsync(Packetlength + len, token);
			//Записываем ID пакета
			await BaseStream.WriteAsync(idData, 0, len, token);


			//Все данные пакета перекидваем в интернет
			await packet.CopyToAsync(BaseStream, token);


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
			//if (disposedStream)
			//{
			//	if (BaseStream is { })
			//	{
			//		BaseStream.Dispose();
			//		BaseStream = null;
			//	}
			//}
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
			//if (disposedStream)
			//{
			//	if (BaseStream is not null)
			//	{
			//		await BaseStream.DisposeAsync();
			//		BaseStream = null;
			//	}
			//}
			GC.SuppressFinalize(this);
		}
	}
}


