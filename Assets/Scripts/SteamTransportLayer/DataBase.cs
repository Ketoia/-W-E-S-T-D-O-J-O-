using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
public class DataMessage
{
    public string Key;
    public DataState State;

    public DataMessage(string key)
    {
        Key = key;
        State = DataState.Event;
    }

    public void SendDataToClients()
    {
        //serialise
        Byte[] bytes;
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            bytes = ms.ToArray();
        }

        if (SteamManager.instance.Connected)
        {
            foreach (var item in SteamManager.instance.server.Connected)
            {
                item.SendMessage(bytes);
            }
        }
    }

    public void SendDataToServer()
    {
        //serialise
        Byte[] bytes;
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            bytes = ms.ToArray();
        }
        SteamManager.instance.connection.Connection.SendMessage(bytes);
    }

}

public enum DataState
{
    Event,
    Variable
}