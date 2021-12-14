using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SyncData<T>
{
    [NonSerialized]
    public static List<SyncData<T>> ObjectsToSynchronise = new List<SyncData<T>>();
    public string Key;

    private T _Value;
    public T Value
    {
        get => _Value;
        set
        {
            if(!_Value.Equals(value))
            {
                _Value = value;
                if (SteamManager.instance.host)
                    SendDataToClients();
                else if(SteamManager.instance.Connected) 
                    SendDataToServer();
            }
        }
    }

    public SyncData(string Key)
    {
        this.Key = Key;
        ObjectsToSynchronise.Add(this);
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

        if(SteamManager.instance.Connected)

        foreach (var item in SteamManager.instance.server.Connected)
        {
            item.SendMessage(bytes);
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

    public static void ReciveData(byte[] arrBytes)
    {
        //deserialise data
        using var memStream = new MemoryStream();
        var binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        SyncData<T> obj = binForm.Deserialize(memStream) as SyncData<T>;

        //Fing Object With Key
        foreach (SyncData<T> objecto in ObjectsToSynchronise)
        {
            if (objecto.Key == obj.Key)
            {
                objecto.Value = obj.Value;
                break;
            }
        }
    }
}