using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GUISkin MyGuiSkin;
    Rect StartPos = new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 100);
    Rect BackPos = new Rect(Screen.width - 90, Screen.height - 40, 80, 50);

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

        if(MyMainMenu != MenuState.MainMenu)
        {
            Back();
        }
    }

    void MainMenu()
    {
        GUILayout.BeginArea(StartPos);
        if (GUILayout.Button("HostGame", MyGuiSkin.customStyles[0])) 
        {
            MyMainMenu = MenuState.HostTheGame;
            SteamManager.instance.HostGame();

        }
        if (GUILayout.Button("Connect to SteamID", MyGuiSkin.customStyles[0]))
        {
            MyMainMenu = MenuState.JoinToTheGame;
            friends = System.Linq.Enumerable.ToList(Steamworks.SteamFriends.GetFriends());
        }
        GUILayout.EndArea();
    }

    void JoinToTheGame()
    {
        GUILayout.BeginArea(StartPos);
        foreach (Steamworks.Friend friend in friends)
        {
            if(friend.IsPlayingThisGame)
            {
                if (GUILayout.Button(friend.Name, MyGuiSkin.customStyles[0]))
                {
                    SteamManager.instance.ConnectToGame(friend.Id);
                }
            }
        }

        if (SteamManager.instance.Connected == true) MyMainMenu = MenuState.ConnectedToHost;
        GUILayout.EndArea();
    }

    void HostTheGame()
    {
        GUILayout.BeginArea(StartPos);
        if (GUILayout.Button("Play", MyGuiSkin.customStyles[0]))
        {
            if (!SteamManager.instance.Connected)
            {
                EventsTransport dataMessage = new ("StartGame");
                dataMessage.SendEventToClients();
            }

            NetworkManager.StartGame();
        }

        List<PlayerData> players = NetworkManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            GUILayout.Box(players[i].Name, MyGuiSkin.customStyles[0]);
        }
        GUILayout.EndArea();
    }

    void ConnectedToHost()
    {
        //test
        GUILayout.Box("wait for play", MyGuiSkin.customStyles[0]);

        List<PlayerData> players = NetworkManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            GUILayout.Box(players[i].Name, MyGuiSkin.customStyles[0]);
        }

        if(!SteamManager.instance.Connected) MyMainMenu = MenuState.MainMenu;
    }

    void Back()
    {
        GUILayout.BeginArea(BackPos);
        if(GUILayout.Button("Back", MyGuiSkin.customStyles[0]))
        {
            if(SteamManager.instance.Connected) SteamManager.instance.connection.Close();
            if (SteamManager.instance.host) SteamManager.instance.server.Close();
            MyMainMenu = MenuState.MainMenu;
        }
        GUILayout.EndArea();

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
