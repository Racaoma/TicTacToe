using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworking : NetworkBehaviour
{
    //Control Variables
    [SyncVar(hook = "onTurnChange")]
    public Player currentTurn = Player.None;

    //References
    private GameLogic gameLogicRef;

    //Singleton
    private static ServerNetworking instance;
    public static ServerNetworking Instance
    {
        get
        {
            return instance;
        }
    }

    //On Object Awake
    private void Awake()
    {
        //Check Singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //On Object Destroy (Safeguard)
    public void OnDestroy()
    {
        instance = null;
    }

    //On Turn Change
    public void onTurnChange(Player currentPlayer)
    {
        currentTurn = currentPlayer;
        GameView.Instance.highlightPlayer(currentPlayer);
        AudioControl.Instance.playGrunt();
    }

    //Start
    private void Start()
    {
        gameLogicRef = this.GetComponent<GameLogic>();
        currentTurn = GameManager.firstMove;
    }

    //Update Turn
    public void updateTurn()
    {
        if (currentTurn == Player.Player1) currentTurn = Player.Player2;
        else currentTurn = Player.Player1;
    }

    //Make Play
    public bool makePlay(int cellNumber, Symbol symbol)
    {
        return gameLogicRef.makePlay(cellNumber, symbol);
    }

    //Check End Game
    public VictoryType checkEndGame()
    {
        return gameLogicRef.checkEndGame();
    }
}
