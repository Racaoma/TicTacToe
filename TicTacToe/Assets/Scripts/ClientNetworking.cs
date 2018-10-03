using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ClientNetworking : NetworkBehaviour
{
    //Sync Variables
    [SyncVar]
    public Player playerNumber;
    [SyncVar]
    public Symbol playerSymbol;
    [SyncVar]
    public Color playerColor;
    [SyncVar(hook = "onClientReady")]
    public bool ready = false;

    //On Start Authority
    public override void OnStartAuthority()
    {
        //Base Method
        base.OnStartAuthority();

        //Set Player
        if (NetworkServer.active) playerNumber = Player.Player1;
        else playerNumber = Player.Player2;

        //Request Sync
        CmdRequestSync(playerNumber);
    }

    //On Ready
    public void onClientReady(bool ready)
    {
        //Only Call on Client
        if (playerNumber == Player.Player2 && ready) CmdRequestGameStart();
    }

    //Get Local ClientNetworking
    public static ClientNetworking getLocalClientNetworking()
    {
        ClientNetworking[] list = FindObjectsOfType<ClientNetworking>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].hasAuthority) return list[i];
        }
        return null;
    }

    //Get Opponent ClientNetworking
    public static ClientNetworking getOpponentClientNetworking()
    {
        ClientNetworking[] list = FindObjectsOfType<ClientNetworking>();
        for (int i = 0; i < list.Length; i++)
        {
            if (!list[i].hasAuthority) return list[i];
        }
        return null;
    }

    //Check Turn
    public bool isMyTurn()
    {
        return ServerNetworking.Instance.currentTurn == playerNumber;
    }

    //Make Play
    [Command]
    public void CmdSendPlay(int cellNumber)
    {
        if (ServerNetworking.Instance.makePlay(cellNumber, playerSymbol))
        {
            RpcRelayPlay(cellNumber, playerSymbol);
            VictoryType victoryCheck = ServerNetworking.Instance.checkEndGame();
            if (victoryCheck != VictoryType.None) RpcRelayWinner(victoryCheck, playerSymbol);

            //Update Turn
            ServerNetworking.Instance.updateTurn();
        }
    }

    [ClientRpc]
    public void RpcRelayPlay(int cellNumber, Symbol symbol)
    {
        //Update States
        int row = cellNumber / 3;
        int column = cellNumber % 3;
        GameState.Instance.updateCell(row, column, symbol);
        GameView.Instance.destroyGhost(cellNumber); //If Any

        if(symbol == playerSymbol) GameView.Instance.updateCell(cellNumber, symbol, playerColor);
        else GameView.Instance.updateCell(cellNumber, symbol, ClientNetworking.getOpponentClientNetworking().playerColor);
    }

    //Display Winner
    [ClientRpc]
    public void RpcRelayWinner(VictoryType victoryType, Symbol winnerSymbol)
    {
        ServerNetworking.Instance.currentTurn = Player.None;
        GameView.Instance.displayWinner(victoryType, winnerSymbol == ClientNetworking.getLocalClientNetworking().playerSymbol);
    }

    //Request Initial Sync
    [Command]
    public void CmdRequestSync(Player player)
    {
        if (player == Player.Player1)
        {
            //Host
            playerNumber = player;
            playerSymbol = GameManager.player1Symbol;
            playerColor = GameManager.player1Color;
            ready = true;
        }
        else
        {
            //Client
            playerNumber = player;
            playerSymbol = GameManager.player2Symbol;
            playerColor = GameManager.player2Color;
            ready = true;

            //Spawn Server
            GameObject obj = Instantiate(MyNetworkManager.singleton.spawnPrefabs[0], Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
    }

    //Request Game Start
    [Command]
    public void CmdRequestGameStart()
    {
        RpcRelayStartGame();
    }

    [ClientRpc]
    public void RpcRelayStartGame()
    {
        FindObjectOfType<GameFlowManager>().startGame();
    }
}
