using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //References
    private ServerNetworking server;

    //Sync Variables
    public Player localPlayer;
    public Symbol localSymbol;
    public Color localColor;

    //Control Variables
    [SyncVar, SerializeField]
    private Player currentTurn;

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
            server = obj.GetComponent<ServerNetworking>();
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
        server.makePlay(cellNumber, localSymbol);
    }

    [ClientRpc]
    public void RpcRelayPlay(int cellNumber, Symbol symbol)
    {
        //Update States
        int row = cellNumber / 3;
        int column = cellNumber % 3;
        GameState.Instance.updateCell(row, column, symbol);
        GameView.Instance.destroyGhost(cellNumber); //If Any
        GameView.Instance.updateCell(cellNumber, symbol);

        //Update Turn
        if (currentTurn == Player.Player1) currentTurn = Player.Player2;
        else currentTurn = Player.Player1;
    }

    //Request Initial Sync
    [Command]
    public void CmdRequestSync(Player player)
    {
        if (player == Player.Player1) TargetRpcRelaySync(connectionToClient,GameManager.player1Symbol, GameManager.player1Color);
        else TargetRpcRelaySync(connectionToClient, GameManager.player2Symbol, GameManager.player2Color);
    }

    [TargetRpc]
    public void TargetRpcRelaySync(NetworkConnection target, Symbol symbolPlayer, Color colorPlayer)
    {
        localSymbol = symbolPlayer;
        localColor = colorPlayer;
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
