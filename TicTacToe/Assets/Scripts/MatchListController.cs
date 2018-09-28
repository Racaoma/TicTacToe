using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchListController : MonoBehaviour
{
    //References
    public GameObject MatchButtonTemplate;
    public GameObject viewPort;
    private MyNetworkManager networkManager;

    //Variables
    public float DiscoveryUpdatePeriod = 0.5f;
    private List<GameObject> matches;
    private float updateTimer;
    private List<NetworkBroadcastResult> broadcastResults;

    //Start
    private void Start()
    {
        //Variables
        matches = new List<GameObject>();
        broadcastResults = new List<NetworkBroadcastResult>();
        updateTimer = 0f;

        //Network
        networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
        networkManager.getNetworkDiscovery().Initialize();
        networkManager.getNetworkDiscovery().StartAsClient();
    }

    //Update
    private void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            RefreshMatches();
            updateTimer = DiscoveryUpdatePeriod;
        }
    }

    //Refresh Matches
    private void RefreshMatches()
    {
        if(networkManager.getNetworkDiscovery().broadcastsReceived != null)
        {
            //Get Broadcasts
            broadcastResults.Clear();
            foreach (var match in networkManager.getNetworkDiscovery().broadcastsReceived.Values)
            {
                broadcastResults.Add(match);
            }

            //Update Buttons
            int i = 0;
            for (; i < broadcastResults.Count; i++)
            {
                //Check for existing buttons
                if (i < matches.Count) matches[i].GetComponent<MatchButton>().updateInfo(broadcastResults[i]);
                else
                {
                    GameObject buttonObject = Instantiate(MatchButtonTemplate);
                    buttonObject.transform.SetParent(viewPort.transform, false);
                    buttonObject.GetComponent<MatchButton>().updateInfo(broadcastResults[i]);
                    matches.Add(buttonObject);
                }
            }

            //Disable extra buttons for closed matches
            for (; i < matches.Count; i++)
            {
                matches[i].SetActive(false);
            }
        }
    }
}
