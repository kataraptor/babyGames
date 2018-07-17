using System;
using UnityEngine;

namespace Hive.Resources
{
	[Serializable]
	public class SocketMessage : ISocketMessage
	{
		public string Command;

		public string Payload;
		public string Player;

		public string SaveToString()
		{
			return JsonUtility.ToJson(this);
		}

		public static SocketMessage CreateFromJson(string jsonString)
		{
			return JsonUtility.FromJson<SocketMessage>(jsonString);
		}
	}
}