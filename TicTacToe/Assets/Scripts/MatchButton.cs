using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchButton : MonoBehaviour
{
    //Variables
    private NetworkBroadcastResult networkData;

    //Update Info
    public void updateInfo(NetworkBroadcastResult networkData)
    {
        GetComponentInChildren<Text>().text = Encoding.Unicode.GetString(networkData.broadcastData);
        this.networkData = networkData;
    }

    //Main Button Logic
    public void Button_Click()
    {
        MyNetworkManager.singleton.networkAddress = networkData.serverAddress;
        MyNetworkManager.singleton.StartClient();
        MyNetworkManager.Discovery.StopBroadcast();
    }
}
