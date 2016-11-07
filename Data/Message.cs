using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class Message
    {
		public static byte[] Pack(int value)
		{
			return BitConverter.GetBytes(value);
		}

		public static byte[] Pack(long value)
		{
			return BitConverter.GetBytes(value);
		}

		public static byte[] Pack(string value)
		{
			List<byte> temp = new List<byte>();

			byte[] bytes = Encoding.Unicode.GetBytes(value);
			temp.AddRange(Pack(bytes.Length));
			temp.AddRange(bytes);

			return temp.ToArray();
		}
    }
}
