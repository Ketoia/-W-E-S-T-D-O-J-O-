using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ZeroFormatter;
using MyData;

public class IMyConnection : IConnectionManager
{
	public void OnConnecting(ConnectionInfo data)
	{
		Debug.Log($"{data.Identity.SteamId} is connecting");
	}

	public void OnConnected(ConnectionInfo data)
	{
		EventsTransport.SingleEventTransport(SteamManager.instance.MyPlayer, (int)SingleEventsSeed.AddPlayer);
		Debug.Log($"{data.Identity.SteamId} has joined the game");
	}

	public void OnDisconnected(ConnectionInfo data)
	{
		ServerInstance.instance.Client = false;
		Debug.Log($"{data.Identity.SteamId} is out of here");
	}

	public void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		byte[] managedArray = new byte[size];
		Marshal.Copy(data, managedArray, 0, size);

		SyncData obj = ZeroFormatterSerializer.Deserialize<SyncData>(managedArray);

		switch (obj.TypeAsString)
		{
			case "Int32":
				SyncDataInt syncDataInt = ZeroFormatterSerializer.Deserialize<SyncDataInt>(managedArray);
				EventManager.TriggerEvent(syncDataInt.Key, syncDataInt.Value);
				break;
			case "Single":
				SyncDataFloat syncDataFloat = ZeroFormatterSerializer.Deserialize<SyncDataFloat>(managedArray);
				EventManager.TriggerEvent(syncDataFloat.Key, syncDataFloat.Value);
				break;
			case "String":
				SyncDataString syncDataString = ZeroFormatterSerializer.Deserialize<SyncDataString>(managedArray);
				EventManager.TriggerEvent(syncDataString.Key, syncDataString.Value);
				break;
			case "Char":
				SyncDataChar syncDataChar = ZeroFormatterSerializer.Deserialize<SyncDataChar>(managedArray);
				EventManager.TriggerEvent(syncDataChar.Key, syncDataChar.Value);
				break;
			case "Boolean":
				SyncDataBool syncDataBool = ZeroFormatterSerializer.Deserialize<SyncDataBool>(managedArray);
				EventManager.TriggerEvent(syncDataBool.Key, syncDataBool.Value);
				break;
			case "Vector3":
				SyncDataVector3 syncDataVector3 = ZeroFormatterSerializer.Deserialize<SyncDataVector3>(managedArray);
				EventManager.TriggerEvent(syncDataVector3.Key, syncDataVector3.Value);
				break;
			case "Vector3Int":
				SyncDataVector3Int syncDataVector3Int = ZeroFormatterSerializer.Deserialize<SyncDataVector3Int>(managedArray);
				EventManager.TriggerEvent(syncDataVector3Int.Key, syncDataVector3Int.Value);
				break;
			case "SyncDataPlayerData":
				SyncDataPlayerData syncDataPlayerData = ZeroFormatterSerializer.Deserialize<SyncDataPlayerData>(managedArray);
				EventManager.TriggerEvent(syncDataPlayerData.Key, syncDataPlayerData);
				break;
			case "ComponentsDataList":
				ComponentsDataList componentsDataList = ZeroFormatterSerializer.Deserialize<ComponentsDataList>(managedArray);
				EventManager.TriggerEvent(componentsDataList.Key, componentsDataList);
				break;
			default:
				Debug.LogWarning("I dont have this type of data: " + obj.TypeAsString);
				break;
		}
	}
}