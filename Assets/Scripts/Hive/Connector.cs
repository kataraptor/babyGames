using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Hive.Resources;
using UnityEngine;

namespace Hive
{
	// This is the client DLL class code to use for the sockServer
	// include this DLL in your Plugins folder under Assets
	// using it is very simple
	// Look at LinkSyncSCR.cs

	namespace SharpConnect
	{
		public class Connector
		{
			private const int ReadBufferSize = 255;
			private TcpClient _client;
			public ArrayList LstUsers = new ArrayList();
			private Action<SocketMessage> _onCommand;
			private string _pUserName;
			private readonly byte[] _readBuffer = new byte[ReadBufferSize];
			public string Res = string.Empty;
			public string StrMessage = string.Empty;

			public string Connect(string sNetIp, int iPortNum, string sUserName, Action<SocketMessage> onCommand)
			{
				try
				{
					Debug.Log(sNetIp);
					Debug.Log(iPortNum);
					_pUserName = sUserName;
					_onCommand = onCommand;
					// The TcpClient is a subclass of Socket, providing higher level 
					// functionality like streaming.
					_client = new TcpClient(sNetIp, iPortNum);
					// Start an asynchronous read invoking DoRead to avoid lagging the user
					// interface.
					Debug.Log("here");
					_client.GetStream().BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
					// Make sure the window is showing before popping up connection dialog.

					AttemptLogin(sUserName);
					return "Connection Succeeded";
				}
				catch (Exception ex)
				{
					return "Server is not active.  Please start server and try again.      " + ex;
				}
			}

			public void AttemptLogin(string user)
			{
				SendData("CONNECT", user);
			}

			public void SendMessage(string command, string message)
			{
				SendData(command, message);
			}

			public void FnDisconnect()
			{
				SendData("DISCONNECT", "");
			}

			public void FnListUsers()
			{
				SendData("REQUESTUSERS", "");
			}

			private void DoRead(IAsyncResult ar)
			{
				Debug.Log ("Do read");
				try
				{
					Debug.Log("try do read");
					// Finish asynchronous read into readBuffer and return number of bytes read.
					var bytesRead = _client.GetStream().EndRead(ar);
					if (bytesRead < 1)
					{
						Debug.Log("Server closed");
						// if no bytes were read server has close.  
						Res = "Disconnected";
						return;
					}
					Debug.Log("String message");
					// Convert the byte array the message was saved into, minus two for the
					// Chr(13) and Chr(10)
					StrMessage = Encoding.ASCII.GetString(_readBuffer, 0, bytesRead);
					Debug.Log(StrMessage);
					ProcessCommands(StrMessage);
					// Start a new asynchronous read into readBuffer.
					_client.GetStream().BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
				}
				catch
				{
					Res = "Disconnected";
				}
			}

			// Process the command received from the server, and take appropriate action.
			private void ProcessCommands(string strMessage)
			{
				var socketMessage = SocketMessage.CreateFromJson(strMessage);
				_onCommand(socketMessage);
				Debug.Log(socketMessage.Command);
				Debug.Log(socketMessage.Payload);
			}

			// Use a StreamWriter to send a message to server.
			private void SendData(string command, string message)
			{
				var time = "temp";
				var socketMessage = new SocketMessage
				{
					Command = command,
					Player = _pUserName,
					Payload = message,
				};
				Debug.Log("testing here: " + socketMessage.SaveToString());
				var writer = new StreamWriter(_client.GetStream());
				writer.Write(socketMessage.SaveToString() + (char) 13);
				writer.Flush();
			}
		}
	}
}