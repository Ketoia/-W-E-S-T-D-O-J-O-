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
            //if(friend.Name == "NooNitron")
            if(friend.IsPlayingThisGame)
            {
                if (GUILayout.Button(friend.Name))
                {
                    SteamManager.instance.ConnectToGame(friend.Id);
                }
            }
        }

        if (GUILayout.Button("me"))
        {
            SteamManager.instance.ConnectToGame(Steamworks.SteamClient.SteamId);
        }

        if (SteamManager.instance.Connected == true) MyMainMenu = MenuState.ConnectedToHost;
    }

    void HostTheGame()
    {
        if (GUILayout.Button("Play"))
        {
            if (!SteamManager.instance.Connected)
            {
                EventsTransport dataMessage = new EventsTransport("StartGame");
                dataMessage.SendEventToClients();
            }

            NetworkManager.StartGame();
        }

        List<PlayerData> players = NetworkManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            GUILayout.Box(players[i].Name);
        }
    }

    void ConnectedToHost()
    {
        //test
        GUILayout.Box("wait for play");

        List<PlayerData> players = NetworkManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            GUILayout.Box(players[i].Name);
        }
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
