using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public enum MultiplayerType
{
    None,
    LAN,
    Internet
}

public class MyNetworkManager : NetworkManager
{
    //Control Variables
    public MultiplayerType multiplayerType;
    public bool loadingLevel;

    //Start
    private void Start()
    {
        multiplayerType = MultiplayerType.None;
        loadingLevel = false;
    }

    //Called on the server when a client disconnects.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (NetworkServer.connections.Count < 2) GameView.Instance.displayPlayerDisconnectedPanel();
    }

    //Called on clients when disconnected from a server.
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        GameView.Instance.displayPlayerDisconnectedPanel();
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success) base.OnMatchJoined(success, extendedInfo, matchInfo);
        else GameView.Instance.displayPlayerDisconnectedPanel();
    }

    //Called on Server when a Client Connects
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (NetworkServer.connections.Count < 2) base.OnServerConnect(conn);
        else if (NetworkServer.connections.Count == 2)
        {
            base.OnServerConnect(conn);
            getNetworkDiscovery().StopBroadcast();
        }
        else conn.Disconnect();
    }

    //Set Multiplayer Type (LAN)
    public void setMultiplayerLAN()
    {
        this.multiplayerType = MultiplayerType.LAN;
    }

    //Set Multiplayer Type (Internet)
    public void setMultiplayerInternet()
    {
        this.multiplayerType = MultiplayerType.Internet;
    }

    //Get Network Discovery
    public NetworkDiscovery getNetworkDiscovery()
    {
        return GetComponent<NetworkDiscovery>();
    }

    //On Host Start
    public override void OnStartHost()
    {
        //Base Method
        base.OnStartHost();

        //Define Symbol Random
        if(GameManager.player1Symbol == Symbol.None)
        {
            if (UnityEngine.Random.value >= 0.5f)
            {
                GameManager.player1Symbol = Symbol.Circle;
                GameManager.player2Symbol = Symbol.Cross;
            }
            else
            {
                GameManager.player1Symbol = Symbol.Cross;
                GameManager.player2Symbol = Symbol.Circle;
            }
        }

        //Define Color Random
        if (UnityEngine.Random.value >= 0.5f)
        {
            GameManager.player1Color = Color.red;
            GameManager.player2Color = Color.blue;
        }
        else
        {
            GameManager.player1Color = Color.blue;
            GameManager.player2Color = Color.red;
        }

        //Define Starting Player Random
        if(GameManager.firstMove == Player.None)
        {
            if (UnityEngine.Random.value >= 0.5f) GameManager.firstMove = Player.Player1;
            else GameManager.firstMove = Player.Player2;
        }
    }
}
