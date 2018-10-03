using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class HostGameButton : MonoBehaviour
{
    //References
    public InputField inputField;
    public Dropdown hostSymbol;
    public Dropdown startingPlayer;
    private MyNetworkManager networkManager;

    //Create Lan Match
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

            //Load Scene
            networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
            if (networkManager.multiplayerType == MultiplayerType.LAN) FindObjectOfType<PanelFlow>().loadGame(createMatchLAN);
            else FindObjectOfType<PanelFlow>().loadGame(createMatchInternet);
        }
    }

    //Create Match LAN
    public void createMatchLAN()
    {
        networkManager.getNetworkDiscovery().StopBroadcast();
        networkManager.getNetworkDiscovery().broadcastData = inputField.text;
        networkManager.getNetworkDiscovery().StartAsServer();
        MyNetworkManager.singleton.StartHost();
    }

    //Create Match Internet
    public void createMatchInternet()
    {
        MyNetworkManager networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
        networkManager.matchMaker.CreateMatch(inputField.text, 2, true, "", "", "", 0, 0, MyNetworkManager.singleton.OnMatchCreate);
    }
}
