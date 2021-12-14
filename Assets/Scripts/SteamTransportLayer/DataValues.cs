using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class DataValues<T> : DataMessage
{
    [NonSerialized]
    public static List<DataValues<T>> ObjectsToSynchronise = new List<DataValues<T>>();

    private T _Value;
    public T Value
    {
        get => _Value;
        set
        {
            if (!_Value.Equals(value))
            {
                _Value = value;
                if (SteamManager.instance.host)
                {
                    SendDataToClients();
                }
                if (SteamManager.instance.Connected)
                    SendDataToServer();
            }
        }
    }

    public DataValues(string key) : base(key)
    {
        Key = key;
        State = DataState.Variable;
        ObjectsToSynchronise.Add(this);
    }

    public static void ReciveData(byte[] arrBytes)
    {
        //deserialise data
        using var memStream = new MemoryStream();
        var binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        var obj = binForm.Deserialize(memStream);

        //Fing Object With Key
        foreach (DataValues<T> objecto in ObjectsToSynchronise)
        {
            if (objecto.Key == (obj as DataValues<T>).Key)
            {
                objecto.Value = (obj as DataValues<T>).Value;
                break;
            }
        }
    }
}
