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
    //Refences
    private NetworkDiscovery Discovery;

    //Control Variables
    private NetworkConnection player1Connection;
    private NetworkConnection player2Connection;
    public MultiplayerType multiplayerType;
    public bool loadingLevel;

    //Start
    private void Start()
    {
        multiplayerType = MultiplayerType.None;
        loadingLevel = false;
    }

    //Set Multiplayer Type
    public void setMultiplayerType(bool internet)
    {
        if(internet) this.multiplayerType = MultiplayerType.Internet;
        else this.multiplayerType = MultiplayerType.LAN;
    }

    //Get Network Discovery
    public NetworkDiscovery getNetworkDiscovery()
    {
        return GetComponent<NetworkDiscovery>();
    }

    //On Host Start
    public override void OnStartHost()
    {
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
