using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //Control Variables
    private Player currentTurn;

    //My Variables
    public Player localPlayer;
    public Symbol localSymbol;
    public Color localColor;

    //Opponent Variables
    public Player opponentPlayer;
    public Symbol opponentSymbol;
    public Color opponentColor;

    //On Start Authority
    public override void OnStartAuthority()
    {
        //Host Side
        if (NetworkServer.active)
        {
            localPlayer = Player.Player1;
            CmdRequestSync(localPlayer);
            currentTurn = GameManager.firstMove;

            //Spawn Server
            GameObject obj = Instantiate(MyNetworkManager.singleton.spawnPrefabs[0], Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(obj);
        }
        //Client Side
        else
        {
            localPlayer = Player.Player2;
            CmdRequestSync(localPlayer);
            CmdRequestStartGame();
        }
    }

    //Get ClientNetworking
    public static ClientNetworking getLocalClientNetworking()
    {
        ClientNetworking[] list = FindObjectsOfType<ClientNetworking>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].hasAuthority) return list[i];
        }
        return null;
    }

    //Check Turn
    public bool isMyTurn()
    {
        return currentTurn == localPlayer;
    }

    //Make Play
    [Command]
    public void CmdSendPlay(int cellNumber)
    {
        ServerNetworking.Instance.makePlay(cellNumber, localSymbol);
    }

    [ClientRpc]
    public void RpcRelayPlay(int cellNumber, Symbol symbol)
    {
        Debug.LogError("RECEIVED PLAY");

        //Update States
        int row = cellNumber / 3;
        int column = cellNumber % 3;
        GameState.Instance.updateCell(row, column, symbol);
        GameView.Instance.destroyGhost(cellNumber); //If Any

        if(symbol == localSymbol) GameView.Instance.updateCell(cellNumber, symbol, localColor);
        else GameView.Instance.updateCell(cellNumber, symbol, opponentColor);

        //Update Turn
        if (currentTurn == Player.Player1) currentTurn = Player.Player2;
        else currentTurn = Player.Player1;
    }

    //Request Initial Sync
    [Command]
    public void CmdRequestSync(Player player)
    {
        if (player == Player.Player1) TargetRpcRelaySync(connectionToClient, GameManager.firstMove, GameManager.player1Symbol, GameManager.player1Color);
        else TargetRpcRelaySync(connectionToClient, GameManager.firstMove, GameManager.player2Symbol, GameManager.player2Color);
    }

    [TargetRpc]
    public void TargetRpcRelaySync(NetworkConnection target, Player firstMove, Symbol symbolPlayer, Color colorPlayer)
    {
        currentTurn = firstMove;
        localSymbol = symbolPlayer;
        localColor = colorPlayer;

        if (localPlayer == Player.Player1) opponentPlayer = Player.Player2;
        else opponentPlayer = Player.Player1;

        if (symbolPlayer == Symbol.Circle) opponentSymbol = Symbol.Cross;
        else opponentSymbol = Symbol.Circle;

        if (colorPlayer == Color.red) opponentColor = Color.blue;
        else opponentColor = Color.red;

        Debug.LogError("SYNC RECEIVED");
        Debug.LogError("Player " + localPlayer + ", Symbol " + localSymbol + ", Turn " + currentTurn);
    }

    //Start Game
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

    //Display Winner
    [ClientRpc]
    public void RpcRelayWinner(VictoryType victoryType, Symbol winnerSymbol)
    {
        GameView.Instance.displayWinner(victoryType, winnerSymbol == localSymbol);
        currentTurn = Player.None;
    }
}
