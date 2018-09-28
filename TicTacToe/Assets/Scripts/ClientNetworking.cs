using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //Sync Variables
    [SyncVar, SerializeField]
    private Player currentTurn;
    [SyncVar]
    public Player playerNumber;
    [SyncVar]
    public Symbol playerSymbol;
    [SyncVar]
    public Color playerColor;

    //On Start Authority
    public override void OnStartAuthority()
    {
        //Host Side
        if (NetworkServer.active)
        {
            Debug.LogError("Server");
            playerNumber = Player.Player1;
            CmdRequestSync(playerNumber);
            currentTurn = GameManager.firstMove;

            //Spawn Server
            GameObject obj = Instantiate(MyNetworkManager.singleton.spawnPrefabs[0], Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
        //Client Side
        else
        {
            Debug.LogError("Client");
            playerNumber = Player.Player2;
            CmdRequestSync(playerNumber);
        }
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
        return currentTurn == playerNumber;
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
            Debug.LogError("Turn Updated!");
            if (currentTurn == Player.Player1) currentTurn = Player.Player2;
            else currentTurn = Player.Player1;
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
        GameView.Instance.displayWinner(victoryType, winnerSymbol == playerSymbol);
        currentTurn = Player.None;
    }

    //Request Initial Sync
    [Command]
    public void CmdRequestSync(Player player)
    {
        currentTurn = GameManager.firstMove;
        if (player == Player.Player1)
        {
            playerNumber = player;
            playerSymbol = GameManager.player1Symbol;
            playerColor = GameManager.player1Color;
        }
        else
        {
            playerNumber = player;
            playerSymbol = GameManager.player2Symbol;
            playerColor = GameManager.player2Color;
            TargetRpcRequestGameStart(connectionToClient);
        }
    }

    //Start Game
    [TargetRpc]
    public void TargetRpcRequestGameStart(NetworkConnection target)
    {
        CmdRequestStartGame();
    }

    [Command]
    public void CmdRequestStartGame()
    {
        RpcRelayStartGame();
    }

    [ClientRpc]
    public void RpcRelayStartGame()
    {
        FindObjectOfType<GameFlowManager>().startGame();
    }
}
