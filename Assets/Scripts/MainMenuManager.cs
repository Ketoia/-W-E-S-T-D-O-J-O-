using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void OnGUI()
    {
        MenuFSM();
    }

    #region MenuSFM

    MenuState MyMainMenu = MenuState.MainMenu;
    List<Steamworks.Friend> friends;

    void MenuFSM()
    {
        switch (MyMainMenu)
        {
            case MenuState.MainMenu:
                MainMenu();
                break;

            case MenuState.JoinToTheGame:
                JoinToTheGame();
                break;

            case MenuState.HostTheGame:
                HostTheGame();
                break;

            case MenuState.ConnectedToHost:
                ConnectedToHost();
                break;
        }
    }

    void MainMenu()
    {
        if (GUILayout.Button("HostGame"))
        {
            MyMainMenu = MenuState.HostTheGame;
            SteamManager.instance.HostGame();

        }
        if (GUILayout.Button("Connect to SteamID"))
        {
            MyMainMenu = MenuState.JoinToTheGame;
            friends = System.Linq.Enumerable.ToList(Steamworks.SteamFriends.GetFriends());
        }
    }

    void JoinToTheGame()
    {
        foreach (Steamworks.Friend friend in friends)
        {
            if(friend.IsPlayingThisGame)
            {
                if (GUILayout.Button(friend.Name))
                {
                    SteamManager.instance.ConnectToGame(friend.Id);
                }
            }
        }
        if (SteamManager.instance.Connected == true) MyMainMenu = MenuState.ConnectedToHost;
    }

    void HostTheGame()
    {
        if (GUILayout.Button("Play"))
        {
            SceneManager.LoadScene(1);
            NetworkManager.instance.StartCoroutine();
        }

        List<Steamworks.Data.Connection> list = System.Linq.Enumerable.ToList(SteamManager.instance.server.Connected);
        for (int i = 0; i < list.Count; i++)
        {
            //GUILayout.Label(list[i].Id.ToString());
            GUILayout.Label(list[i].DetailedStatus());
        }
    }

    void ConnectedToHost()
    {
        //test
        Debug.Log("wait for play");
    }

    enum MenuState
    {
        MainMenu,
        JoinToTheGame,
        HostTheGame,
        ConnectedToHost
    }
    #endregion
}
