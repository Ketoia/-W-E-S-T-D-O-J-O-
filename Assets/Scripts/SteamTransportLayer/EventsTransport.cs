using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class EventsTransport
{
    public string Key;
    public object Object;

    public EventsTransport(string key)
    {
        Key = key;
    }

    public EventsTransport(string key, object Object)
    {
        Key = key;
        this.Object = Object;
    }

    public void SendEventTo(Steamworks.Data.Connection Con)
    {
        //serialise
        Byte[] bytes;
        BinaryFormatter bf = new();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            bytes = ms.ToArray();
        }

        Con.SendMessage(bytes);        
    }

    public void SendEventToClients()
    {
        //serialise
        Byte[] bytes;
        BinaryFormatter bf = new();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            bytes = ms.ToArray();
        }

        foreach (var item in SteamManager.instance.server.Connected)
        {
            item.SendMessage(bytes);
        }
    }

    public void SendEventToServer()
    {
        if (SteamManager.instance.host) return;
        //serialise
        Byte[] bytes;
        BinaryFormatter bf = new();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            bytes = ms.ToArray();
        }
        SteamManager.instance.connection.Connection.SendMessage(bytes);
    }

    public void AutomaticEventSend()
    {
        if (SteamManager.instance.host)
            SendEventToClients();
        else SendEventToServer();
    }
}
