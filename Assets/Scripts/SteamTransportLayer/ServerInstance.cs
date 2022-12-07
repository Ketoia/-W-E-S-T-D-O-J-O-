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

    [SerializeField]
    private List<NetworkObject> NetworkObjects = new List<NetworkObject>();

    protected void Start()
    {
        instance = this;

        EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DuplicateNetworkObject), DuplicateNetGameObject);
        EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.AddComponent), AddNetworkComponent);
        EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DeleteComponent), DeleteComponent);
        EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DeleteNetworkObject), DeleteNetGameobject);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DuplicateNetworkObject), DuplicateNetGameObject);
        EventManager.StopListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.AddComponent), AddNetworkComponent);
        EventManager.StopListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DeleteComponent), DeleteComponent);
        EventManager.StopListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.DeleteNetworkObject), DeleteNetGameobject);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConnectToGame(SteamClient.SteamId);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            string c = new string('1', (int)Mathf.Pow(2,24f) - 50);
            EventsTransport a = new EventsTransport(Guid.NewGuid());
            a.Value = c;
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
        ComponentsDataList value = (ComponentsDataList)Value;

        GameObject NewNetworkedGameObject = new GameObject("Object" + NetworkObjects.Count.ToString());
        NetworkObject netobj = NewNetworkedGameObject.AddComponent<NetworkObject>();
        netobj.ParentGuid = value.ParentID;
        netobj.IsMaster = false;

        NetworkObject.AddNetworkedComponent(netobj, value.Value);

        NetworkObjects.Add(netobj);
    }

    public void DeleteNetGameobject(object Value)
    {
        ComponentsDataList value = (ComponentsDataList)Value;

        GameObject gameObject = NetworkObjects.Find(e => e.ParentGuid == value.ParentID).gameObject;
        if (gameObject != null)
            Destroy(gameObject);
        else Debug.Log("GameObject not found!");
    }

    public void DeleteComponent(object Value)
    {
        ComponentsDataList value = (ComponentsDataList)Value;

        NetworkObject netobj = NetworkObjects.Find(e => e.ParentGuid == value.ParentID);

        NetworkObject.DeleteNetworkedComponent(netobj, value.Value);
    }

    public void AddNetworkComponent(object Value)
    {
        ComponentsDataList value = (ComponentsDataList)Value;

        NetworkObject netobj = NetworkObjects.Find(e => e.ParentGuid == value.ParentID);
        if (netobj != null)
            NetworkObject.AddNetworkedComponent(netobj, value.Value);
        else Debug.LogError("Netorked Object not Found!");
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
        if (tet == null || tet.TypeAsString == "ComponentsDataList") DataToSync.Add(eventsTrans);
        else tet = eventsTrans;
    }

    public float tickrate = 0.025f;

    IEnumerator ServerTick()
    {
        while (Host || Client) 
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
        
        //if(Server != null) Server.Close();
        //if(Connection != null) Connection.Close();
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
                case "ComponentsDataList":
                    bytes = ZeroFormatterSerializer.Serialize((ComponentsDataList)syncData);
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