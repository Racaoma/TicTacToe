using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //Variables
    private Player player;
    private GameLogic gameLogicRef;
    private bool player1Ready;

    //Start
    private void Start()
    {
        CmdSyncSetup();
        if (NetworkServer.active) player = Player.Player1;
        else
        {
            player = Player.Player2;
            CmdRequestStartGame();
        }
    }

    //Check which Player you are
    public Player getPlayer()
    {
        return player;
    }

    [Command]
    public void CmdSendPlay(int cellNumber)
    {
        RpcRelayPlay(cellNumber);
    }

    [ClientRpc]
    public void RpcRelayPlay(int cellNumber)
    {
        gameLogicRef.makePlay(cellNumber);
    }

    [Command]
    public void CmdSyncSetup()
    {
        RpcRelaySyncSetup(GameManager.typePlayer1, GameManager.typePlayer2, GameManager.firstMove, GameManager.player1Symbol, GameManager.player2Symbol, GameManager.player1Color, GameManager.player2Color);
    }

    [ClientRpc]
    public void RpcRelaySyncSetup(PlayerType p1type, PlayerType p2type, Player firstMove, Symbol player1Symbol, Symbol player2Symbol, Color player1Color, Color player2Color)
    {
        gameLogicRef = FindObjectOfType<GameLogic>();
        GameManager.typePlayer1 = p1type;
        GameManager.typePlayer2 = p2type;
        GameManager.firstMove = firstMove;
        GameManager.player1Symbol = player1Symbol;
        GameManager.player2Symbol = player2Symbol;
        GameManager.player1Color = player1Color;
        GameManager.player2Color = player2Color;
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
