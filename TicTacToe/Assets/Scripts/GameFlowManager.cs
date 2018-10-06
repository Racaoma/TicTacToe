using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    //References
    public AudioControl audioControl;
    public GameObject boardObject;

    //Return to Main Menu
    public void loadTitleScreen(bool fade)
    {
        if (fade) StartCoroutine(audioControl.FadeMusic());
        else
        {
            //Break Connection
            MyNetworkManager.singleton.StopHost();

            //Load Title Screen Scene
            SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        }
    }

    //Setup Game
    public void startGame()
    {
        //Start Board
        boardObject.SetActive(true);

        //Start View
        GameView view = this.GetComponent<GameView>();
        view.setupGameView();

        //Reset Board
        GameState.Instance.resetBoard();
        view.resetBoard();

        //Start Music
        audioControl.playMusic();
    }
}
