using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    [HideInInspector]
    public static List<PlayerData> Players = new List<PlayerData>();
    public List<GameObject> PlayersGameObjects = new List<GameObject>();

    public static NetworkManager instance;

    public Sprite sprite;
    private void Start()
    {
        instance = this;
        EventManager.StartListening("AddPlayer", AddPlayer);
        EventManager.StartListening("StartGame", StartGame);
    }

    public void StartCoroutine()
    {
        StartCoroutine(CreatePlayers());
    }
    public IEnumerator CreatePlayers()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(1).isLoaded);
        PlayersGameObjects.Add(Instantiate(PlayerPrefab));
        for (int i = 0; i < Players.Count; i++)
        {
            GameObject player = new GameObject(Players[i].Name);
            player.AddComponent<OtherPlayer>().InitialGameObject(Players[i], sprite);
            PlayersGameObjects.Add(player);
        }
    }

    public static void AddPlayer(object player)
    {
        Players.Add((PlayerData)player);
    }

    public static void StartGame()
    {
        SceneManager.LoadScene(1);
        instance.StartCoroutine(instance.CreatePlayers());
    }
}
