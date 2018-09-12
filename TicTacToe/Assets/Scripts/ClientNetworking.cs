using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //Variables
    private GameLogic gameLogicRef;

    //Start
    private void Start()
    {
        gameLogicRef = FindObjectOfType<GameLogic>();
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
}
