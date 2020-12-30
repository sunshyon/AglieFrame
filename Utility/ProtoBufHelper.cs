using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	/// <summary>
	/// ProtoBuf序(反)列化，速度更快是json 5倍
	/// NuGet :protobuf-net
	/// </summary>
	public static class ProtoBufHelper
    {
		/// <summary>
		/// 序列化
		/// </summary>
		public static string Serialize<T>(T t)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				Serializer.Serialize<T>(ms, t);
				return Encoding.UTF8.GetString(ms.ToArray());
			}
		}
		/// <summary>
		/// 反序列化
		/// </summary>
		public static T DeSerialize<T>(string data)
		{
			var content= Encoding.UTF8.GetBytes(data);
			using (MemoryStream ms = new MemoryStream(content))
			{
				T t = Serializer.Deserialize<T>(ms);
				return t;
			}
		}
	}

	/// <summary>
	/// 以下是ProtoBuf序列化操作必须的
	/// </summary>
	[ProtoContract]
	public class UserDemo
	{
		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public int Age { get; set; }
		[ProtoMember(3)]
		public string Phone { get; set; }
		[ProtoMember(4)]
		public string Address { get; set; }
		[ProtoMember(5)]
		public string Email { get; set; }
	}
}
