using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchListController : MonoBehaviour
{
    //References
    public GameObject MatchButtonTemplate;
    public GameObject viewPort;

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
        MyNetworkManager.Discovery.Initialize();
        MyNetworkManager.Discovery.StartAsClient();
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
        //Get Broadcasts
        broadcastResults.Clear();
        foreach (var match in MyNetworkManager.Discovery.broadcastsReceived.Values)
        {
            broadcastResults.Add(match);
        }

        //Update Buttons
        int i = 0;
        for (; i < broadcastResults.Count; i++)
        {
            //Check for existing buttons
            if(i < matches.Count) matches[i].GetComponent<MatchButton>().updateInfo(broadcastResults[i]);
            else
            {
                GameObject buttonObject = Instantiate(MatchButtonTemplate);
                buttonObject.transform.SetParent(viewPort.transform);
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
