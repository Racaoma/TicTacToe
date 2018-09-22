using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    //Variables
    public NetworkDiscovery Discovery;

    //Get Network Discovery
    public NetworkDiscovery getNetworkDiscovery()
    {
        return GetComponent<NetworkDiscovery>();
    }

    //On Server Connect
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (conn.address == "localClient") return;
        //else do something
    }
}
