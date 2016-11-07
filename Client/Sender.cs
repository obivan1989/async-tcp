using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Sender : IDisposable
    {
		private string _ip;
		private int _port;

		private TcpClient _client;
		private NetworkStream _stream;

		public Sender(string ip, int port)
		{
			_ip = ip;
			_port = port;
			_client = new TcpClient();
		}

		public async Task Connect()
		{	
			if (!_client.Connected)
			{
				try
				{
					await _client.ConnectAsync(_ip, _port);
					_stream = _client.GetStream();
				}
				catch (Exception e) 
				{
 
				}
			}
		}

		public async Task Send(byte[] data)
		{
			await Connect();
			await _stream.WriteAsync(data, 0, data.Length);
		}

		public void Dispose()
		{
			if(_stream != null)
			{
				_stream.Close();
				_stream.Dispose();
			}

			if(_client != null)
			{
				_client.Close();
			}
		}
	}
}
