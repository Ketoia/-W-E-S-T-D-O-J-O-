using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public ulong SteamID;

    public PlayerData(string Name, ulong SteamID)
    {
        this.Name = Name;
        this.SteamID = SteamID;
    }
}
