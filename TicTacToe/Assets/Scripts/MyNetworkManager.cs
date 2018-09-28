using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    //Refences
    private NetworkDiscovery Discovery;

    //Control Variables
    private NetworkConnection player1Connection;
    private NetworkConnection player2Connection;

    //Get Network Discovery
    public NetworkDiscovery getNetworkDiscovery()
    {
        return GetComponent<NetworkDiscovery>();
    }

    //On Host Start
    public override void OnStartHost()
    {
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

    //On Client Connect
    public override void OnClientConnect(NetworkConnection conn) { } //Do Nothing

    //Called on the server when a client is ready
    public override void OnServerReady(NetworkConnection conn)
    {
        if (player1Connection == null) player1Connection = conn;
        else
        {
            player2Connection = conn;
            GameObject player1Obj = GameObject.Instantiate(MyNetworkManager.singleton.playerPrefab);
            GameObject player2Obj = GameObject.Instantiate(MyNetworkManager.singleton.playerPrefab);
            NetworkServer.AddPlayerForConnection(player1Connection, player1Obj, 1);
            NetworkServer.AddPlayerForConnection(player2Connection, player2Obj, 2);
        }
    }

    //On Scene Change
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        ClientScene.Ready(conn);
    }
}
