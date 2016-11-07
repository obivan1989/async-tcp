using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	public abstract class Client : IDisposable
	{
		private Socket _socket;

		public Socket Socket
		{
			set { _socket = value; }
		}

		public Client()
		{
			_socket = null;
		}

		public async Task Process()
		{
			using (NetworkStream stream = new NetworkStream(_socket, true))
			{
				try
				{
					do
					{
						byte[] buff = new byte[sizeof(int)];
						int read = await stream.ReadAsync(buff, 0, buff.Length);
						int size = BitConverter.ToInt32(buff, 0) - sizeof(int);
						if (size > 0)
						{
							buff = new byte[size];
							int offset = 0;
							do { offset += await stream.ReadAsync(buff, offset, buff.Length - offset); } while (offset != buff.Length);
							Accept(buff);
						}
					} while (true);
				}
				catch (Exception e)
				{

				}
			}
		}

		public async virtual Task Accept(byte[] buff)
		{
			Console.WriteLine(Encoding.UTF8.GetString(buff));
		}

		public void Dispose()
		{
			if (_socket != null)
			{
				_socket.Close();
				_socket.Dispose();
			}
		}
	}
}
