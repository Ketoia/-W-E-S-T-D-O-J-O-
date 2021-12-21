using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class IMyConnection : IConnectionManager
{
	public void OnConnecting(ConnectionInfo data)
	{
		Debug.Log($"{data.Identity.SteamId} is connecting");
	}

	public void OnConnected(ConnectionInfo data)
	{
		SteamManager.instance.Connected = true;
		EventsTransport dataMessage = new EventsTransport("AddPlayer", SteamManager.instance.MyPlayer);
		dataMessage.SendEventToServer();

		Debug.Log($"{data.Identity.SteamId} has joined the game");
	}

	public void OnDisconnected(ConnectionInfo data)
	{
		SteamManager.instance.Connected = false;
		//Debug.Log($"{data.Identity.SteamId} is out of here");
	}

	public void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		Debug.Log("Receive Message from server");
		byte[] managedArray = new byte[size];
		Marshal.Copy(data, managedArray, 0, size);

		//deserialise data
		using var memStream = new MemoryStream();
		var binForm = new BinaryFormatter();
		memStream.Write(managedArray, 0, managedArray.Length);
		memStream.Seek(0, SeekOrigin.Begin);
		EventsTransport obj = binForm.Deserialize(memStream) as EventsTransport;

		if (obj.Object == null)
			EventManager.TriggerEvent(obj.Key);
		else
			EventManager.TriggerEvent(obj.Key, obj.Object);
	}

	//SyncData<object>.ReciveData(managedArray);
	// Send it right back
	//base.OnMessage(data, size, messageNum, recvTime, channel);
}



