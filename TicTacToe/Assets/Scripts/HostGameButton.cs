using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostGameButton : MonoBehaviour
{
    //References
    public InputField inputField;
    public Symbol startingPlayer;
    public Symbol hostSymbol;

    //Create Match
    public void CreateMatch()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            MyNetworkManager networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
            networkManager.getNetworkDiscovery().StopBroadcast();
            networkManager.getNetworkDiscovery().broadcastData = inputField.text;
            networkManager.getNetworkDiscovery().StartAsServer();
            MyNetworkManager.singleton.StartHost();
        }
    }
}
