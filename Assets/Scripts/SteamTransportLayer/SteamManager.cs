using System;
using UnityEngine;
using Steamworks;
using UnityEngine.Events;

public class SteamManager : MonoBehaviour
{
    IMySever MyServerInterface = new IMySever();
    IMyConnection MyConnectionInterface = new IMyConnection();
    public SocketManager server;
    public ConnectionManager connection;

    [HideInInspector]
    public bool host = false;
    [HideInInspector]
    public bool Connected = false;

    public static SteamManager instance;
    public PlayerData MyPlayer;

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

        MyPlayer = new PlayerData(SteamClient.Name, SteamClient.SteamId.Value);
    }

    void Update()
    {
        if (host)
        {
            server.Receive();
        }

        if (Connected) 
        {
            connection.Receive();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ConnectToGame(SteamClient.SteamId);
        }
    }

    private void OnDestroy()
    {
        if(host) server.Close();
        SteamClient.Shutdown();
    }

    public void HostGame()
    {
        server = SteamNetworkingSockets.CreateRelaySocket(0, MyServerInterface);
        host = true;
    }

    public void ConnectToGame(SteamId steamId)
    {
        connection = SteamNetworkingSockets.ConnectRelay(steamId, 0, MyConnectionInterface);
    }
}