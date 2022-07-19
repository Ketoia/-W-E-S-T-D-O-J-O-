using System;
using System.Collections;
using UnityEngine;
using Steamworks;
using UnityEngine.Events;
using MyData;
public class SteamManager : MonoBehaviour
{
    public static SteamManager instance;
    public SyncDataPlayerData MyPlayer;

    private void Awake()
    {
        try
        {
            SteamClient.Init(1789450);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        if (instance == null) instance = this;
        DontDestroyOnLoad(this);

        MyPlayer = new SyncDataPlayerData();
        MyPlayer.Name = SteamClient.Name;
        MyPlayer.SteamID = SteamClient.SteamId;
    }

    private void OnDestroy()
    {
        SteamClient.Shutdown();
    }
}