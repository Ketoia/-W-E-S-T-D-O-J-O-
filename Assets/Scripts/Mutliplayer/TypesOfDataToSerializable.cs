using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroFormatter;

namespace MyData
{
    [ZeroFormattable]
    public class SyncData
    {
        [Index(0)]
        public virtual Guid Key { get; set; }
        [Index(1)]
        public virtual string TypeAsString { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataString : SyncData
    {
        [Index(2)]
        public virtual string Value { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataChar : SyncData
    {
        [Index(2)]
        public virtual char Value { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataInt : SyncData
    {
        [Index(2)]
        public virtual int Value { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataFloat : SyncData
    {
        [Index(2)]
        public virtual float Value { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataBool : SyncData
    {
        [Index(2)]
        public virtual bool Value { get; set; }
    }

    [ZeroFormattable]
    public class SyncDataVector3 : SyncData
    {
        [Index(2)]
        public virtual Vector3 Value { get; set; }
    }

    [System.Serializable]
    [ZeroFormattable]
    public struct Vector3
    {
        [Index(0)]
        public float x;
        [Index(1)]
        public float y;
        [Index(2)]
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
            {
                return false;
            }

            var s = (Vector3)obj;
            return x == s.x &&
                   y == s.y &&
                   z == s.z;
        }

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }

        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static implicit operator UnityEngine.Vector3(Vector3 x)
        {
            return new UnityEngine.Vector3(x.x, x.y, x.z);
        }
        public static implicit operator Vector3(UnityEngine.Vector3 x)
        {
            return new Vector3(x.x, x.y, x.z);
        }

        public static bool operator ==(Vector3 a, UnityEngine.Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3 a, UnityEngine.Vector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

    }

    [ZeroFormattable]
    public class SyncDataVector3Int : SyncData
    {
        [Index(2)]
        public virtual Vector3Int Value { get; set; }
    }

    [ZeroFormattable]
    public struct Vector3Int
    {
        [Index(0)]
        public int x;
        [Index(1)]
        public int y;
        [Index(2)]
        public int z;

        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

        }
    }

    [ZeroFormattable]
    public class SyncDataPlayerData : SyncData
    {
        [Index(2)]
        public virtual string Name { get; set; }
        [Index(3)]
        public virtual ulong SteamID { get; set; }
    }

    [ZeroFormattable]
    public class ComponentsDataList : SyncData
    {
        [Index(2)]
        public virtual Guid ParentID { get; set; }
        [Index(3)]
        public virtual List<ComponentsData> Value { get; set; }
    }

    [ZeroFormattable]
    [Serializable]
    public struct ComponentsData
    {
        [Index(0)]
        public Guid ComponentID { get; set; }
        [Index(1)]
        public string TypeAsString { get; set; }
        [Index(2)]
        public MyTransportObject MyTransportObjects { get; set; }

        public ComponentsData(Guid id, string typeAsString, MyTransportObject myTransportObjects/*List<Guid> traposrtObjectKeys, List<string> traposrtObjectTypesAsString*/)
        {
            ComponentID = id;
            TypeAsString = typeAsString;
            MyTransportObjects = myTransportObjects;
        }
    }

    [ZeroFormattable]
    [Serializable]
    public class MyTransportObject
    {
        [Index(0)]
        public virtual List<Guid> Id { get; set; }
        [Index(1)]
        public virtual List<string> TypeAsString { get; set; }
    }

    public static class MyDataDefaultVariables
    {
        public static object GetDefaultVariable(string type)
        {
            switch(type)
            {
                case "Int32":
                    return (int)0;
                    //return default(int);
                case "Single":
                    return (float)0.0f;
                    //return default(float);
                case "String":
                    return (string)"";
                    //return default(string);
                case "Char":
                    return (char)0;
                    //return default(char);
                case "Boolean":
                    return (bool)false;
                    //return default(bool);
                case "Vector3":
                    return new Vector3(0, 0, 0);
                    //return default(Vector3);
                case "Vector3Int":
                    return new Vector3Int(0, 0, 0);
                //return default(Vector3Int);
                default:
                    Debug.LogWarning("I dont have this type of data: " + type);
                    break;
            }
            return null;
        }
    }
}
