using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    [HideInInspector]
    public List<GameObject> players = new List<GameObject>();
    public static NetworkManager instance;

    private void Start()
    {
        instance = this;
    }

    public void StartCoroutine()
    {
        StartCoroutine(CreatePlayers());
    }
    public IEnumerator CreatePlayers()
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(1).isLoaded);
        players.Add(Instantiate(PlayerPrefab));
    }

    public void StartGame()
    {

    }
}
