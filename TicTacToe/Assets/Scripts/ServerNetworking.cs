using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNetworking : MonoBehaviour
{
    //References
    private GameLogic gameLogicRef;

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

    //Start
    private void Start()
    {
        gameLogicRef = this.GetComponent<GameLogic>();
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
