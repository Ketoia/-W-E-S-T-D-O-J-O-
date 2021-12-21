using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class IMySever : ISocketManager
{
	public void OnConnecting(Connection connection, ConnectionInfo data)
	{
		connection.Accept();
		Debug.Log($"{data.Identity.SteamId} is connecting");
	}

	public void OnConnected(Connection connection, ConnectionInfo data)
	{
		Debug.Log($"{data.Identity.SteamId} has joined the game");

		//ToDo
		EventsTransport dataMessage = new EventsTransport("AddPlayer", SteamManager.instance.MyPlayer);
		dataMessage.SendEventToClients();

	}

	public void OnDisconnected(Connection connection, ConnectionInfo data)
	{
		//Debug.Log($"{data.Identity.SteamId} is out of here");
	}

	public void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		Debug.Log("Receive Message from connect");

		byte[] managedArray = new byte[size];
		Marshal.Copy(data, managedArray, 0, size);

		//deserialise data
		using var memStream = new MemoryStream();
		var binForm = new BinaryFormatter();
		memStream.Write(managedArray, 0, managedArray.Length);
		memStream.Seek(0, SeekOrigin.Begin);
		EventsTransport obj = binForm.Deserialize(memStream) as EventsTransport;

		if(obj.Object == null)
			EventManager.TriggerEvent(obj.Key);
		else
			EventManager.TriggerEvent(obj.Key, obj.Object);

		//Todo
		// Send it to other connected boys
		foreach (Connection conncted in SteamManager.instance.server.Connected)
        {
			if(conncted != connection) conncted.SendMessage(data, size);
		}
	}
}
