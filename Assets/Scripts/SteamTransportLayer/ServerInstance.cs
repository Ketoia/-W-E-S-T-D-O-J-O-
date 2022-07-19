using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeroFormatter;
using MyData;
using Steamworks;

public class ServerInstance : MonoBehaviour
{
    private bool isCoroutinerunning = false;
    
    public List<SyncData> DataToSync = new();
    public static ServerInstance instance;

    private IMySever MyServerInterface = new();
    private IMyConnection MyConnectionInterface = new();
    public SocketManager Server;
    public ConnectionManager Connection;

    public bool Host = false;
    public bool Client = false;

    private List<NetworkObject> NetworkObjects = new List<NetworkObject>();

    protected void Start()
    {
        instance = this;

        EventManager.StartListening(EventsTransport.GenerateSeededGuid(3), DuplicateNetGameObject);
        EventManager.StartListening(EventsTransport.GenerateSeededGuid(4), AddNetworkComponent);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConnectToGame(SteamClient.SteamId);
        }
    }

    protected void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    protected void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    #region Multi Tools
    public void HostGame()
    {
        Server = SteamNetworkingSockets.CreateRelaySocket(0, MyServerInterface);
        Host = true;
        StartServerTick();
    }

    public void ConnectToGame(SteamId steamId)
    {
        Connection = SteamNetworkingSockets.ConnectRelay(steamId, 0, MyConnectionInterface);
        Client = true;
        instance.StartServerTick();
    }

    public void DuplicateNetGameObject(object Value)
    {
        GameObject a = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        NetworkObject netobj = a.AddComponent<NetworkObject>();
        netobj.ParentGuid = Guid.Parse((string)Value);

        NetworkObjects.Add(netobj);

        ComponentDataList comp = new ComponentDataList();
        comp.Key = netobj.ParentGuid;
        comp.ComponentTypeAsString = "NetworkBehaviour";
        comp.Value = new List<SyncData>();
        comp.Value.Add(new SyncData() { Key = Guid.NewGuid(), TypeAsString = "string" });
        //comp.Value = new List<SyncData> { }
        //AddNetworkComponent(comp);
    }

    public void AddNetworkComponent(object Value)
    {
        ComponentDataList value = (ComponentDataList)Value;
        Type ComponentType = Type.GetType(value.ComponentTypeAsString);

        if (ComponentType == null)
        {
            Debug.LogError("There is nothing like: " + (string)Value);
            return;
        }

        GameObject a = NetworkObjects.Find(e => e.ParentGuid == value.Key).gameObject;// GameObject.CreatePrimitive(PrimitiveType.Sphere);
        NetworkBehaviour comp = (NetworkBehaviour)a.AddComponent(ComponentType);
        comp.OnBehaviourAdd();
        var list = comp.transports;

        foreach (SyncData data in value.Value)
        {
            list.Add(new EventsTransport("", data.Key));
        }
    }
    #endregion

    #region RepairingCoroutine;
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            instance = this;
            StartServerTick();
        }
    }

    #endregion

    #region Syncronising Data

    public void StartServerTick()
    {
        if(!isCoroutinerunning)
        {
            isCoroutinerunning = true;
            StartCoroutine(ServerTick());
        }
    }

    public void AddToDataList(SyncData eventsTrans)
    {
        SyncData tet = DataToSync.Find(e => e.Key == eventsTrans.Key);
        if (tet == null) DataToSync.Add(eventsTrans);
        else tet = eventsTrans;
    }

    public float tickrate = 0.025f;
    IEnumerator ServerTick()
    {
        while(Host || Client)
        {
            yield return null;//new WaitForSeconds(tickrate); //tickratio 

            SendData();
            if (Host)
            {
                Server.Receive();
            }

            if (Client)
            {
                Connection.Receive();
            }
        }
        isCoroutinerunning = false;
        Server.Close();
    }

    void SendData()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        if (DataToSync.Count == 0) return;

        List<SyncData> data = new List<SyncData>(DataToSync);

        foreach (SyncData syncData in data)
        {
            byte[] bytes = new byte[0];
            switch (syncData.TypeAsString)
            {
                case "Int32":
                    //Debug.Log("You have got an Int");
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataInt)syncData);
                    break;
                case "Single":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataFloat)syncData);
                    break;
                case "String":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataString)syncData);
                    break;
                case "Char":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataChar)syncData);
                    break;
                case "Boolean":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataBool)syncData);
                    break;
                case "Vector3":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataVector3)syncData);
                    break;
                case "Vector3Int":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataVector3Int)syncData);
                    break;
                case "SyncDataPlayerData":
                    bytes = ZeroFormatterSerializer.Serialize((SyncDataPlayerData)syncData);
                    break;
                case "CopyComponent":
                    bytes = ZeroFormatterSerializer.Serialize((ComponentDataList)syncData);
                    break;
                default:
                    Debug.LogWarning("I dont have this type of data: " + syncData.TypeAsString);
                    break;
            }

            if (Host)
            {
                foreach (var item in Server.Connected)
                {
                    item.SendMessage(bytes);
                }
            }
            else if (Client)
            {
                Connection.Connection.SendMessage(bytes);
            }
            DataToSync.Remove(syncData);

            if (stopwatch.ElapsedMilliseconds > 1) break;
        }
    }
    #endregion
}