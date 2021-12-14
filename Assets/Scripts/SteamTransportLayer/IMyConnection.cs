using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Runtime.InteropServices;

public class IMyConnection : IConnectionManager
{
	public void OnConnecting(ConnectionInfo data)
	{
		//Debug.Log($"{data.Identity.SteamId} is connecting");
	}

	public void OnConnected(ConnectionInfo data)
	{
		SteamManager.instance.Connected = true;
		//Debug.Log($"{data.Identity.SteamId} has joined the game");
	}

	public void OnDisconnected(ConnectionInfo data)
	{
		SteamManager.instance.Connected = false;
		//Debug.Log($"{data.Identity.SteamId} is out of here");
	}

	public void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		byte[] managedArray = new byte[size];
		Marshal.Copy(data, managedArray, 0, size);

		//SyncData<object>.ReciveData(managedArray);
		// Send it right back
		//base.OnMessage(data, size, messageNum, recvTime, channel);
	}
}


