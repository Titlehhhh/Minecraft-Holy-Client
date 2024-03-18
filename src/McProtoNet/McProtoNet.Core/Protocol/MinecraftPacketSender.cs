using Microsoft.IO;
using System.Buffers;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketSender : IMinecraftPacketSender,IDisposable
	{
		public Stream BaseStream { get; set; }

		public MinecraftPacketSender(Stream baseStream) : base()
		{
			BaseStream = baseStream;
			//compressedPacket = StaticResources.MSmanager.GetStream();
			//zLib = new ZLibStream(compressedPacket, CompressionMode.Compress);
		}

		//private MemoryStream compressedPacket;
		//private ZLibStream zLib;
		public MinecraftPacketSender()
		{
			//compressedPacket = StaticResources.MSmanager.GetStream();
			//zLib = new ZLibStream(compressedPacket, CompressionMode.Compress);
		}

		private static ArrayPool<byte> TestPool = ArrayPool<byte>.Shared;


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
				//ThrowIfDisposed();




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

			//ThrowIfDisposed();
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

			//ThrowIfDisposed();
			int id = packet.Id;
			var data = packet.Data;
			data.Position = 0;
			//await semaphore.WaitAsync(token);
			try
			{


				if (_compressionThreshold > 0)
				{


					byte[] idData = TestPool.Rent(5);
					try
					{
						int idLen = id.GetVarIntLength(idData);


						int uncompressedSize = idLen + (int)data.Length;
						if (uncompressedSize >= _compressionThreshold)
						{
							
							using (var compressedPacket = StaticResources.MSmanager.GetStream())
							{
								
								

								using (var zlibStream = new ZLibStream(compressedPacket, CompressionMode.Compress, true))
								{

									//await zLib.WriteVarIntAsync(id, token);

									await zlibStream.WriteAsync(idData, 0, idLen, token);

									await data.CopyToAsync(zlibStream, token);
									//await zlibStream.FlushAsync(token);

									
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
							await BaseStream.WriteAsync(idData, 0, idLen, token);

							await data.CopyToAsync(BaseStream, token);


						}
					}
					finally
					{
						TestPool.Return(idData);
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

		private async ValueTask SendPacketWithoutCompressionAsync(MemoryStream packet, int id, CancellationToken token)
		{
			//ThrowIfDisposed();
			// packet.Write(writer);
			int Packetlength = (int)packet.Length;

			var idDataMemory = TestPool.Rent(5);
			try
			{

				int len = id.GetVarIntLength(idDataMemory);

				if (len > 5)
					throw new Exception("var int big");


				await BaseStream.WriteVarIntAsync(Packetlength + len, token);

				await BaseStream.WriteAsync(idDataMemory, 0, len, token);


				await packet.CopyToAsync(BaseStream, token);
			}
			finally
			{
				TestPool.Return(idDataMemory);
			}


		}
		#endregion
		//~MinecraftPacketSender()
		//{
		//	Dispose();
		//}
		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}
		bool disposed;
		public void Dispose()
		{
			if (disposed)
				return;
			disposed = true;
			//compressedPacket.Dispose();
			//zLib.Dispose();
			//compressedPacket = null;
			//zLib = null;
			GC.SuppressFinalize(this);
		}

		//private bool _disposed;
		//private void ThrowIfDisposed()
		//{
		//	if (_disposed)
		//		throw new ObjectDisposedException(nameof(MinecraftPacketSender));
		//}

		//public void Dispose()
		//{

		//	if (_disposed)
		//		return;
		//	if (semaphore is not null)
		//	{
		//		semaphore.Dispose();
		//		semaphore = null;
		//	}
		//	//if (disposedStream)
		//	//{
		//	//	if (BaseStream is { })
		//	//	{
		//	//		BaseStream.Dispose();
		//	//		BaseStream = null;
		//	//	}
		//	//}
		//	_disposed = true;
		//	GC.SuppressFinalize(this);
		//}

		//public async ValueTask DisposeAsync()
		//{
		//	if (_disposed)
		//		_disposed = true;
		//	if (semaphore is not null)
		//	{
		//		semaphore.Dispose();
		//		semaphore = null;
		//	}
		//	//if (disposedStream)
		//	//{
		//	//	if (BaseStream is not null)
		//	//	{
		//	//		await BaseStream.DisposeAsync();
		//	//		BaseStream = null;
		//	//	}
		//	//}
		//	GC.SuppressFinalize(this);
		//}
	}
}


