using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    //Variables
    public static NetworkDiscovery Discovery;

    //Get Network Discovery
    public NetworkDiscovery getNetworkDiscovery()
    {
        return singleton.GetComponent<NetworkDiscovery>();
    }

    //On Server Connect
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (conn.address == "localClient") return;
        //else do something
    }
}
