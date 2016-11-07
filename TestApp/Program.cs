using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Do().Wait();
		}

		private static async Task Do()
		{
			Server.Listener<MyClient> server = new Server.Listener<MyClient>("127.0.0.1", 3586, @"errors\socket");

			using (Client.Sender sender = new Client.Sender("127.0.0.1", 3586))
			{
				string msg = Console.ReadLine();
				while (!string.IsNullOrEmpty(msg))
				{
					List<byte> data = new List<byte>();
					data.AddRange(Encoding.Unicode.GetBytes(msg));
					int length = (data.Count + sizeof(int));
					data.InsertRange(0, BitConverter.GetBytes(length));
					await sender.Send(data.ToArray());

					msg = Console.ReadLine();
				}
			}
		}
	}

	class MyClient : Server.Client
	{
		public override async Task Accept(byte[] buff)
		{
			Console.WriteLine("Client say: " + Encoding.Unicode.GetString(buff));
		}
	}
}
