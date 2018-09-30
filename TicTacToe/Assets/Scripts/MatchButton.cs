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

    //Update Info
    public void updateInfo(NetworkBroadcastResult networkData)
    {
        GetComponentInChildren<Text>().text = Encoding.Unicode.GetString(networkData.broadcastData);
        this.networkDataBroadcast = networkData;
    }

    //Update Info
    public void updateInfo(MatchInfoSnapshot networkData)
    {
        GetComponentInChildren<Text>().text = networkData.name;
        this.networkDataMatch = networkData;
    }

    //Main Button Logic
    public void EnterMatch()
    {
        if (networkDataMatch != null)
        {
            MyNetworkManager.singleton.matchMaker.JoinMatch(networkDataMatch.networkId, "", "", "", 0, 0, MyNetworkManager.singleton.OnMatchJoined);
        }
        else
        {
            MyNetworkManager.singleton.networkAddress = networkDataBroadcast.serverAddress;
            MyNetworkManager.singleton.StartClient();
            MyNetworkManager networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
            networkManager.getNetworkDiscovery().StopBroadcast();
        }

        //Load Game Scene
        FindObjectOfType<PanelFlow>().loadGame();
    }
}
