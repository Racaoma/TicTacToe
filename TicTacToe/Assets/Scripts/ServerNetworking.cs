using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNetworking : MonoBehaviour
{
    //References
    private ClientNetworking client;
    private GameLogic gameLogicRef;

    //Start
    private void Start()
    {
        client = ClientNetworking.getLocalClientNetworking();
        gameLogicRef = this.GetComponent<GameLogic>();
    }

    //Make Play
    public void makePlay(int cellNumber, Symbol symbol)
    {
        gameLogicRef.makePlay(cellNumber, symbol);
        client.RpcRelayPlay(cellNumber, symbol);

        VictoryType victoryType = gameLogicRef.checkEndGame();
        if (victoryType != VictoryType.None) client.RpcRelayWinner(victoryType, symbol);
    }
}
