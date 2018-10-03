using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchButton : MonoBehaviour
{
    //Variables
    private NetworkBroadcastResult networkDataBroadcast;
    private MatchInfoSnapshot networkDataMatch;
    private MyNetworkManager networkManager;

    //Update Info (LAN)
    public void updateInfo(NetworkBroadcastResult networkData)
    {
        GetComponentInChildren<Text>().text = Encoding.Unicode.GetString(networkData.broadcastData);
        this.networkDataBroadcast = networkData;
    }

    //Update Info (Internet)
    public void updateInfo(MatchInfoSnapshot networkData)
    {
        GetComponentInChildren<Text>().text = networkData.name;
        this.networkDataMatch = networkData;
    }

    //Main Button Logic
    public void EnterMatch()
    {
        //Load Game Scene
        networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
        if(networkManager.multiplayerType == MultiplayerType.LAN) FindObjectOfType<PanelFlow>().loadGame(connectLAN);
        else if(networkManager.multiplayerType == MultiplayerType.Internet) FindObjectOfType<PanelFlow>().loadGame(connectInternet);
    }

    //Connect
    public void connectLAN()
    {
        networkManager.networkAddress = networkDataBroadcast.serverAddress;
        networkManager.StartClient();
        networkManager.getNetworkDiscovery().StopBroadcast();
    }

    //Connect
    public void connectInternet()
    {
        networkManager.matchMaker.JoinMatch(networkDataMatch.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }
}
