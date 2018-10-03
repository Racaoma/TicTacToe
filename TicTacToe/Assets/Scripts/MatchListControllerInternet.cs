using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchListControllerInternet : MonoBehaviour
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
        requestRefresh();
    }

    //Request Refresh
    public void requestRefresh()
    {
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, RefreshMatches);
    }

    //Refresh Matches
    private void RefreshMatches(bool success, string extendedInfo, List<MatchInfoSnapshot> searchResults)
    {
        if(!networkManager.loadingLevel)
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
                matches[i].SetActive(false);
            }
        }
    }
}
