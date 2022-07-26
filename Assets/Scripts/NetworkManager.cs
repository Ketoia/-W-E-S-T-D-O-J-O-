using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyData;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    [HideInInspector]
    public static List<SyncDataPlayerData> Players = new();
    public List<GameObject> PlayersGameObjects = new();

    public static NetworkManager instance;

    public List<Sprite> sprites;
    public Material OutlineMat;

    private void Start()
    {
        instance = this;
        EventManager.StartListening(EventsTransport.GenerateSeededGuid(0), AddPlayer);
        EventManager.StartListening(EventsTransport.GenerateSeededGuid(1), StartGame);
    }

    public IEnumerator CreatePlayers(int SceneIndex)
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName("Gameplay").isLoaded);

        PlayersGameObjects.Add(Instantiate(PlayerPrefab));
        //for (int i = 0; i < Players.Count; i++)
        //{
        //    GameObject player = new(Players[i].Name);
        //    player.AddComponent<OtherPlayer>().InitialGameObject(Players[i], sprites, OutlineMat);
        //    PlayersGameObjects.Add(player);
        //}
    }

    public static void AddPlayer(object player)
    {
        if(!Players.Contains((SyncDataPlayerData)player))
            Players.Add((SyncDataPlayerData)player);
    }

    public static void StartGame(object SceneIndex)
    {
        int sceneIndex = (int)SceneIndex;
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        instance.StartCoroutine(instance.CreatePlayers(sceneIndex));
    }
}
