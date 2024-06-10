using System.Net;
using System.Net.Sockets;
using System.Text;

namespace McProtoNet.Utils
{
	//public delegate void ServerFindedHandler(LanServer server);

	public sealed class LanServerDetector
	{
		//public event ServerFindedHandler ServerFinded;

		private const ushort Port = 4445;
		private const string Host = "224.0.2.60";


		private readonly Socket udpSocket;
		private EndPoint localEndPoint;
		private IPEndPoint localIPEndPoint;
		public LanServerDetector(int ttl)
		{
			//IPHostEntry HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
			//if (HosyEntry.AddressList.Length > 0)
			//{
			//    foreach (IPAddress ip in HosyEntry.AddressList)
			//    {
			//        
			//        strIP = ip.ToString();
			//        cmbInterfaces.Items.Add(strIP);
			//    }
			//}     
			udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			localIPEndPoint = new IPEndPoint(IPAddress.Any, Port);
			localEndPoint = localIPEndPoint;

			//init Socket properties:
			udpSocket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 1);

			//allow for loopback testing 
			udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

			//extremly important to bind the Socket before joining multicast groups 
			udpSocket.Bind(localIPEndPoint);

			//set multicast flags, sending flags - TimeToLive (TTL) 
			// 0 - LAN 
			// 1 - Single Router Hop 
			// 2 - Two Router Hops... 
			udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);

			//join multicast group 
			udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(Host), IPAddress.Broadcast));



		}

		public void StartReceiving()
		{
			StateObject state = new StateObject();
			state.WorkSocket = udpSocket;
			Recieve(state);
		}

		private void Recieve(StateObject state)
		{
			try
			{
				Socket client = state.WorkSocket;
				client.BeginReceiveFrom(state.Buffer, 0, StateObject.BufferSize, 0, ref localEndPoint, new AsyncCallback(ReceiveCallback), state);
			}
			catch
			{

			}
		}


		private void Notify(string data, EndPoint endPoint)
		{
			System.Diagnostics.Debug.WriteLine("n");
			try
			{
				IPEndPoint ip = (IPEndPoint)endPoint;
				string motd = ParseMotd(data);
				ushort port = ushort.Parse(ParseAddress(data));
				//ServerFinded?.Invoke(new LanServer(ip.Address.ToString(), port, motd));
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Lan Server error: " + e);
			}
			finally
			{

			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{


			StateObject state = null;
			try
			{
				state = (StateObject)ar.AsyncState;
				Socket client = state.WorkSocket;


				int bytesRead = client.EndReceiveFrom(ar, ref localEndPoint);


				byte[] bufferCopy = new byte[bytesRead];

				Array.Copy(state.Buffer, 0, bufferCopy, 0, bytesRead);

				Notify(Encoding.UTF8.GetString(bufferCopy), localEndPoint);

				for (int i = 0; i < bytesRead; i++)
					state.Buffer[i] = (byte)'\0';
				Recieve(state);
			}
			catch (Exception e)
			{
				if (state != null)
					Recieve(state);
				else
					StartReceiving();
			}
		}

		public void Stop()
		{
			udpSocket.Close();
		}



		public LanServerDetector() : this(0)
		{

		}

		private static string ParseMotd(string line)
		{
			System.Diagnostics.Debug.WriteLine("data: " + line);
			int n = line.IndexOf("[MOTD]");
			if (n < 0)
			{
				return "missing no";
			}
			int n2 = line.IndexOf("[/MOTD]", n + "[MOTD]".Length);
			if (n2 < n)
			{
				return "missing no";
			}

			return line.JavaSubStr(n + "[MOTD]".Length, n2);
		}


		private static string ParseAddress(string line)
		{
			int n = line.IndexOf("[/MOTD]");
			if (n < 0)
			{
				return null;
			}
			int n2 = line.IndexOf("[/MOTD]", n + "[/MOTD]".Length);
			if (n2 >= 0)
			{
				return null;
			}
			int n3 = line.IndexOf("[AD]", n + "[/MOTD]".Length);
			if (n3 < 0)
			{
				return null;
			}
			int n4 = line.IndexOf("[/AD]", n3 + "[AD]".Length);
			if (n4 < n3)
			{
				return null;
			}

			return line.JavaSubStr(n3 + "[AD]".Length, n4);
		}
	}
	internal static class JavaStringExt
	{
		public static string JavaSubStr(this string self, int startIndex, int endIndex)
		{
			return self.Substring(startIndex, endIndex - startIndex);
		}
		public static string JavaSubStr(this string self, int startIndex)
		{
			return self.Substring(startIndex);
		}
	}
	internal class StateObject
	{
		public const int BufferSize = 1024;

		private byte[] sBuffer;
		private Socket workSocket;

		internal byte[] Buffer
		{
			get { return sBuffer; }
			set { sBuffer = value; }
		}

		internal Socket WorkSocket
		{
			get { return workSocket; }
			set { workSocket = value; }
		}

		internal StateObject()
		{
			sBuffer = new byte[BufferSize];
			workSocket = null;
		}

		internal StateObject(int size, Socket sock)
		{
			sBuffer = new byte[size];
			workSocket = sock;
		}
	}

}
