using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.IO.Compression;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketSender : IMinecraftPacketSender, IDisposable
	{

		public Stream BaseStream { get; set; }

		public MinecraftPacketSender(Stream baseStream) : base()
		{
			BaseStream = baseStream;

		}

		private static ArrayPool<byte> VarIntPool = ArrayPool<byte>.Create(10, 20);





		public MinecraftPacketSender()
		{

		}


		private const int ZERO_VARLENGTH = 1;
		private static readonly byte[] ZERO_VARINT = { 0 };
		//private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);


		public void SendPacket(Packet packet)
		{


			int id = packet.Id;
			var data = packet.Data;
			try
			{


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
							//compressedPacket.GetBuffer()



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


					byte[] buffer = VarIntPool.Rent(10);
					try
					{
						var memory = buffer.AsMemory();

						byte idLen = id.GetVarIntLength(memory);



						int uncompressedSize = idLen + (int)data.Length;
						if (uncompressedSize >= _compressionThreshold)
						{

							using (var compressedPacket = StaticResources.MSmanager.GetStream())
							{
								using (var zlibStream = new ZLibStream(compressedPacket, CompressionLevel.Optimal, true))
								{
									await zlibStream.WriteAsync(memory.Slice(0, idLen), token);
									await data.CopyToAsync(zlibStream, token);
								}

								int compressedPacketLength = (int)compressedPacket.Length;


								byte test = uncompressedSize.GetVarIntLength();
								byte uncompressedSizeLength = uncompressedSize.GetVarIntLength(memory);




								int fullSize = uncompressedSizeLength + compressedPacketLength;

								byte fullsize_len = fullSize.GetVarIntLength(memory.Slice(uncompressedSizeLength));

#if RELEASE
								await BaseStream.WriteAsync(memory.Slice(uncompressedSizeLength, fullsize_len), token);

								await BaseStream.WriteAsync(memory.Slice(0, uncompressedSizeLength), token);
#elif DEBUG
								await BaseStream.WriteVarIntAsync(fullSize, token);

								await BaseStream.WriteVarIntAsync(uncompressedSize, token);
#endif

								compressedPacket.Position = 0;
								await compressedPacket.CopyToAsync(BaseStream, token);

							}
						}
						else
						{
							uncompressedSize++;

							byte unc_len = uncompressedSize.GetVarIntLength(memory.Slice(idLen));

							//await BaseStream.WriteVarIntAsync(uncompressedSize, token);

							await BaseStream.WriteAsync(memory.Slice(idLen, unc_len), token);

							await BaseStream.WriteAsync(ZERO_VARINT, token);

							await BaseStream.WriteAsync(memory.Slice(0, idLen), token);
							//await BaseStream.WriteAsync(buffer, 0, idLen, token);

							await data.CopyToAsync(BaseStream, token);


						}
					}
					finally
					{
						VarIntPool.Return(buffer);
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
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private async ValueTask SendPacketWithoutCompressionAsync(MemoryStream packet, int id, CancellationToken token)
		{
			//ThrowIfDisposed();
			// packet.Write(writer);
			int Packetlength = (int)packet.Length;

			var buffer = VarIntPool.Rent(10);

			try
			{

				byte id_len = id.GetVarIntLength(buffer);

				byte fullsize_len = (id_len + Packetlength).GetVarIntLength(buffer, id_len);


				await BaseStream.WriteAsync(buffer, id_len, fullsize_len, token);
				await BaseStream.WriteAsync(buffer, 0, id_len, token);

				//await BaseStream.WriteVarIntAsync(Packetlength + len, token);

				//await BaseStream.WriteAsync(idDataMemory, 0, len, token);



				await packet.CopyToAsync(BaseStream, token);
			}
			finally
			{
				VarIntPool.Return(buffer);
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


