using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string Name;
    public ulong SteamID;

    public Player(string Name, ulong SteamID)
    {
        this.Name = Name;
        this.SteamID = SteamID;
    }
}
