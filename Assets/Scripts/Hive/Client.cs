using Hive.Resources;
using Hive.SharpConnect;
using UnityEngine;

namespace Hive
{
	public class Client : MonoBehaviour
	{
		public Connector HiveConnection = new Connector();
		public Transform PlayerCoord;
		public string PlayerName = "Annonymous";
		public string Address = "127.0.0.1";

		protected void Start()
		{
			PlayerName = PlayerPrefs.GetString("PlayerName", "PlayerOne");
			Address = PlayerPrefs.GetString("Address", "127.0.0.1");
			Debug.Log(HiveConnection.Connect(Address, 5678, PlayerName, OnCommand));
			if (HiveConnection.Res != "")
			{
				Debug.Log(HiveConnection.Res);
			}

//			EventManager.Instance.AddListener<OutgoingSisEvent>(OutgoingEvent);
		}

		private void OnCommand(SocketMessage message)
		{
//			EventManager.Instance.QueueEvent(new IncomingSisEvent(message));
			Debug.Log("Zephyr Event Manager here: " + message.Command + "-" + message.Payload);
		}

//		private void OutgoingEvent(OutgoingSisEvent evt)
//		{
//			SisConnection.SendMessage(evt.Command, evt.Message);
//		}

		protected void OnApplicationQuit()
		{
			try
			{
				HiveConnection.FnDisconnect();
			}
			catch
			{
				// ignored
			}
		}
	}
}