using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyData;
public class MainMenuManager : MonoBehaviour
{
    public GUISkin MyGuiSkin;
    Rect StartPos = new(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 100);
    Rect BackPos = new(Screen.width - 90, Screen.height - 40, 80, 50);

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
            ServerInstance.instance.HostGame();

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
                    ServerInstance.instance.ConnectToGame(friend.Id);
                }
            }
        }

        if (ServerInstance.instance.Client == true) MyMainMenu = MenuState.ConnectedToHost;
        GUILayout.EndArea();
    }

    void HostTheGame()
    {
        GUILayout.BeginArea(StartPos);
        if (GUILayout.Button("Play", MyGuiSkin.customStyles[0]))
        {
            if (!ServerInstance.instance.Client)
            {
                //EventsTransport dataMessage = new ("StartGame");
                EventsTransport.SoloEventTransport(1, 1);
            }

            NetworkManager.StartGame(1);
        }

        List<SyncDataPlayerData> players = NetworkManager.Players;
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

        List<SyncDataPlayerData> players = NetworkManager.Players;
        for (int i = 0; i < players.Count; i++)
        {
            GUILayout.Box(players[i].Name, MyGuiSkin.customStyles[0]);
        }

        if(!ServerInstance.instance.Client) MyMainMenu = MenuState.MainMenu;
    }

    void Back()
    {
        GUILayout.BeginArea(BackPos);
        if(GUILayout.Button("Back", MyGuiSkin.customStyles[0]))
        {
            if(ServerInstance.instance.Client) ServerInstance.instance.Connection.Close();
            if (ServerInstance.instance.Host) ServerInstance.instance.Server.Close();
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
