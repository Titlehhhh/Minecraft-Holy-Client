using DotNext;
using DotNext.Buffers;
using DotNext.IO;
using LibDeflate;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using McProtoNet.Experimental;
using Org.BouncyCastle.Utilities;
using QuickProxyNet;
using System.Buffers;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;


internal partial class Program
{

	public static async Task Main(string[] args)
	{

		byte[] output = new byte[1024];

		byte[] input = Encoding.UTF8.GetBytes("Asd sdfbsb bfhbfbsd fgsdb fshydfb sdbfyw gufbwegfuwgbh2487rhy 3ib48y 63vt h3 t73 t 37t c28r y-3yt813yv-t8y13]ty v9387yt8 3vy79y v124y 53y4 5vv135 y4 7v52531y58v13t5v918y75t385v1t5v713ty4587 t47v95t1 35t v714 vt43 87v51vt 57813y57 45v15yv7 35 8v135");

		ZlibCompressor compressor = new ZlibCompressor(6);


		int length = compressor.GetBound(input.Length);

		int written = compressor.Compress(input, output);


	}







}
