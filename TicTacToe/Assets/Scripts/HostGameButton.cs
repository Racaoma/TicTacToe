using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostGameButton : MonoBehaviour
{
    //References
    public InputField inputField;
    public Dropdown hostSymbol;
    public Dropdown startingPlayer;

    //Create Match
    public void CreateMatch()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            //Setup
            if (hostSymbol.value == 0) GameManager.player1Symbol = Symbol.None;
            else if (hostSymbol.value == 1) GameManager.player1Symbol = Symbol.Circle;
            else if (hostSymbol.value == 2) GameManager.player1Symbol = Symbol.Cross;

            if (startingPlayer.value == 0) GameManager.firstMove = Player.None;
            else if (startingPlayer.value == 1) GameManager.firstMove = Player.Player1;
            else if (startingPlayer.value == 2) GameManager.firstMove = Player.Player2;

            //Network
            MyNetworkManager networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
            networkManager.getNetworkDiscovery().StopBroadcast();
            networkManager.getNetworkDiscovery().broadcastData = inputField.text;
            networkManager.getNetworkDiscovery().StartAsServer();
            MyNetworkManager.singleton.StartHost();

            //Load Scene
            FindObjectOfType<PanelFlow>().loadGame();
        }
    }
}
