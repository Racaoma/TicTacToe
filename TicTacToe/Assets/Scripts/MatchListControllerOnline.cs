using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchListControllerOnline : MonoBehaviour
{
    //References
    public GameObject MatchButtonTemplate;
    public GameObject viewPort;
    private MyNetworkManager networkManager;

    //Variables
    public float DiscoveryUpdatePeriod = 3f;
    List<GameObject> matches;
    private float updateTimer;

    //Start
    private void Start()
    {
        //Variables
        matches = new List<GameObject>();
        updateTimer = 0f;

        //Network
        networkManager = MyNetworkManager.singleton.GetComponent<MyNetworkManager>();
        if (networkManager.matchMaker == null) networkManager.StartMatchMaker();
    }

    //Update
    private void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, RefreshMatches);
            updateTimer = DiscoveryUpdatePeriod;
        }
    }

    //Request Refresh
    public void requestRefresh()
    {
        updateTimer = 0f;
    }

    //Refresh Matches
    private void RefreshMatches(bool success, string extendedInfo, List<MatchInfoSnapshot> searchResults)
    {
        //Update Buttons
        int i = 0;
        for (; i < searchResults.Count; i++)
        {
            //Check for existing buttons
            if (i < matches.Count) matches[i].GetComponent<MatchButton>().updateInfo(searchResults[i]);
            else
            {
                GameObject buttonObject = Instantiate(MatchButtonTemplate);
                buttonObject.transform.SetParent(viewPort.transform, false);
                buttonObject.GetComponent<MatchButton>().updateInfo(searchResults[i]);
                matches.Add(buttonObject);
            }
        }

        //Disable extra buttons for closed matches
        for (; i < matches.Count; i++)
        {
            if(matches[i] != null) matches[i].SetActive(false);
        }
    }
}
