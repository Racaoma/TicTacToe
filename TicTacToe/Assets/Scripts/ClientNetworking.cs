using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworking : NetworkBehaviour
{
    //Variables
    private GameLogic gameLogicRef;
    private GameView gameViewRef;
    private bool player1Ready;

    //Start
    private void Start()
    {
        //player1Ready = false;
        //gameLogicRef = FindObjectOfType<GameLogic>();
        //gameViewRef = FindObjectOfType<GameView>();
    }

    public override void OnStartAuthority()
    {
        //CmdSetup();
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
    public void CmdSetup()
    {
        player1Ready = true;
        TargetRpcRelaySetup(connectionToClient);
    }

    [TargetRpc]
    public void TargetRpcRelaySetup(NetworkConnection target)
    {
        gameLogicRef.enabled = true;
        gameViewRef.enabled = true;
    }
}
