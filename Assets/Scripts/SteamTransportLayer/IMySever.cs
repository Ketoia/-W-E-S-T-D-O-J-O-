using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Runtime.InteropServices;
using MyData;
using ZeroFormatter;

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
		EventsTransport dataMessage = new(SteamManager.instance.MyPlayer, 0);

	}

	public void OnDisconnected(Connection connection, ConnectionInfo data)
	{
		//Debug.Log($"{data.Identity.SteamId} is out of here");
	}

	public void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
	{
		//Debug.Log("Receive Message from connect");

		//byte[] managedArray = new byte[size];
		//Marshal.Copy(data, managedArray, 0, size);
		//
		////deserialise data
		//using var memStream = new MemoryStream();
		//var binForm = new BinaryFormatter();
		//memStream.Write(managedArray, 0, managedArray.Length);
		//memStream.Seek(0, SeekOrigin.Begin);
		//List<EventsTransport> objlist = binForm.Deserialize(memStream) as List<EventsTransport>;
		//
		////GCHandle handle = (GCHandle)data;
		////List<EventsTransport> objlist = handle.Target as List<EventsTransport>;
		//
		//foreach (var obj in objlist)
		//{
		//	if (obj.Value == null)
		//		EventManager.TriggerEvent(obj.Key);
		//	else
		//		EventManager.TriggerEvent(obj.Key, obj.Value);
		//}
		//
		////Todo
		//// Send it to other connected boys
		//foreach (Connection conncted in ServerInstance.instance.Server.Connected)
		//{
		//	if(conncted != connection) conncted.SendMessage(data, size);
		//}
		//handle.Free();

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
			case "List`1":
				ComponentsDataList copyComponent = ZeroFormatterSerializer.Deserialize<ComponentsDataList>(managedArray);
				EventManager.TriggerEvent(copyComponent.Key, copyComponent.Value);
				break;
			default:
				Debug.LogWarning("I dont have this type of data: " + obj.TypeAsString);
				break;
		}
	}
}
