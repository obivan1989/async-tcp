using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Listener<T> : IDisposable where T : Client, new()
    {
		private bool _stop;

		private int _port;
		private string _ip;

		private string _errorPath;

		private TcpListener _listener;
		private ConcurrentBag<T> _clients;

		public Listener(string ip, int port, string errorPath)
		{
			_stop = false;

			_port = port;
			_ip = ip;
			_errorPath = errorPath;

			_listener = new TcpListener(IPAddress.Parse(ip), port);
			_listener.Start(100);
			Task.Factory.StartNew(this.Listen);
		}

		private async void Listen()
		{
			while(!_stop)
			{
				try 
				{
					Socket socket = await _listener.AcceptSocketAsync();
					Process(socket);
				}
				catch (Exception e) 
				{
					File.AppendAllText(_errorPath, e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
				}
			}
		}

		private async Task Process(Socket socket)
		{
			using (T client = new T())
			{
				try
				{
					client.Socket = socket;
					await client.Process();
				}
				catch (Exception e) 
				{
 
				}
			}
		}

		public void Stop()
		{
			_stop = true;
		}

		public void Dispose()
		{
			Stop();
			if(_listener != null)
				_listener.Stop();
		}
    }
}
