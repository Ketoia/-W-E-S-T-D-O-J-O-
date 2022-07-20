using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using ZeroFormatter;
using MyData;

[Serializable]
public class EventsTransport
{
    [SerializeField]
    Guid Key = Guid.Empty;

    [SerializeField]
    private object _Value = new object();
    public object Value
    {
        get
        {
            return _Value;
        }
        set
        {
            if(value == null || !value.Equals(_Value))
            {                
                _Value = value;

                SyncData data = GenerateSyncData(value, Key);
                ServerInstance.instance.AddToDataList(data);
            }
        }
    }

    public EventsTransport(object value)
    {
        Key = Guid.NewGuid();
        Value = value;
    }

    public EventsTransport(object value, int KeySeed)
    {
        Key = GenerateSeededGuid(KeySeed);
        Value = value;
    }

    public EventsTransport(object value, Guid key)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// It's only for single Events
    /// </summary>
    /// <param name="KeySeed"></param>
    public EventsTransport(Guid key)
    {
        Key = key;
    }

    public Guid GetKey()
    {
        return Key;
    }

    /// <summary>
    /// 0 - Add Player
    /// 1 - Start Game
    /// 2 - Update Frame
    /// 3 - Duplicate NetworkObject
    /// 4 - AddComponent
    /// </summary>
    /// <param name="value"></param>
    /// <param name="KeySeed"></param>
    public static void SoloEventTransport(object value, int KeySeed)
    {
        Guid guid = GenerateSeededGuid(KeySeed);
        SyncData data = GenerateSyncData(value, guid);
        ServerInstance.instance.AddToDataList(data);
    }

    private static SyncData GenerateSyncData(object value, Guid Key)
    {
        switch (value.GetType().Name)
        {
            case "Int32":
                //Debug.Log("You have got an Int");
                SyncDataInt syncDataInt = new SyncDataInt() { Key = Key, Value = (int)value, TypeAsString = "Int32" };
                return syncDataInt;
            case "Single":
                //Debug.Log("You have got an Float");
                SyncDataFloat syncDataFloat = new SyncDataFloat() { Key = Key, Value = (float)value, TypeAsString = "Single" };
                return syncDataFloat;
            case "String":
                //Debug.Log("You have got an string");
                SyncDataString syncDataString = new SyncDataString() { Key = Key, Value = (string)value, TypeAsString = "String" };
                return syncDataString;
            case "Char":
                //Debug.Log("You have got an Float");
                SyncDataChar syncDataChar = new SyncDataChar() { Key = Key, Value = (char)value, TypeAsString = "Char" };
                return syncDataChar;
            case "Boolean":
                //Debug.Log("You have got an Float");
                SyncDataBool syncDataBool = new SyncDataBool() { Key = Key, Value = (bool)value, TypeAsString = "Boolean" };
                return syncDataBool;
            case "Vector3":
                //Debug.Log("You have got an Float");
                SyncDataVector3 syncDataVector3 = new SyncDataVector3() { Key = Key, Value = (MyData.Vector3)value, TypeAsString = "Vector3" };
                return syncDataVector3;
            case "Vector3Int":
                //Debug.Log("You have got an Float");
                SyncDataVector3Int syncDataVector3Int = new SyncDataVector3Int() { Key = Key, Value = (MyData.Vector3Int)value, TypeAsString = "Vector3Int" };
                return syncDataVector3Int;
            case "SyncDataPlayerData":
                SyncDataPlayerData PlayerData = new SyncDataPlayerData() { Key = Key, Name = ((SyncDataPlayerData)value).Name, SteamID = ((SyncDataPlayerData)value).SteamID, TypeAsString = "SyncDataPlayerData" };
                return PlayerData;
            case "List`1":
                ComponentsDataList copyComponent = new ComponentsDataList() { Key = Key, TypeAsString = "List`1", Value = (List<ComponentsData>)value};
                return copyComponent;
            default:
                Debug.LogWarning("I dont have this type of data: " + value.GetType().Name);
                break;
        }
        return null;
    }

    public static Guid GenerateSeededGuid(int seed)
    {
        var r = new System.Random(seed);
        var guid = new byte[16];
        r.NextBytes(guid);

        return new Guid(guid);
    }
}